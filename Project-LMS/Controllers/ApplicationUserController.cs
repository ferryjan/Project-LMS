using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_LMS.Models;
using Project_LMS.ViewModels;

namespace Project_LMS.Controllers
{
    public class ApplicationUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUser
        public ActionResult Index(string search)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var teacherRole = roleManager.FindByName("Teacher");

            if (search == "" || search == null)
            {
                var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == teacherRole.Id)).ToList();
                return View(list);
            }
            else
            {
                var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == teacherRole.Id)).Where(i => i.GivenName.ToLower().Contains(search.ToLower()) || i.FamilyName.ToLower().Contains(search.ToLower()) || i.Email.ToLower().Contains(search.ToLower())).ToList();
                return View(list);
            }

        }

        public ActionResult StudentIndex(int id)
        {
            ViewBag.CourseId = id;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var studentRole = roleManager.FindByName("Student");
            var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).Where(t => t.CourseId == id).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
            return PartialView(list);
        }

        // GET: ApplicationUser/Create
        public ActionResult CreateStudent(int id)
        {
            ViewBag.CourseId = id;
            ViewBag.UserExist = "";
            return View();
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(int id, [Bind(Include = "GivenName,FamilyName,Email")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {

                applicationUser.TimeOfRegistration = DateTime.Now;
                applicationUser.UserName = applicationUser.Email;
                applicationUser.CourseId = id;

                if (db.Users.Any(u => u.UserName == applicationUser.Email))
                {
                    ViewBag.UserExist = "This email is existed in the database, try another one!";
                    ViewBag.CourseId = id;
                    return View(applicationUser);
                }

                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var result = userManager.Create(applicationUser, "Ante_007");
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }

                userManager.AddToRole(applicationUser.Id, "Student");
                return RedirectToAction("Edit", "TeacherCourses", new { id = id });

            }

            ViewBag.UserExist = "";
            ViewBag.CourseId = id;
            return View(applicationUser);
        }

        // GET: ApplicationUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUser/Create
        public ActionResult Create()
        {
            ViewBag.UserExist = "";
            return View();
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GivenName,FamilyName,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {

                applicationUser.TimeOfRegistration = DateTime.Now;
                applicationUser.UserName = applicationUser.Email;

                if (db.Users.Any(u => u.UserName == applicationUser.Email))
                {
                    ViewBag.UserExist = "This email is existed in the database, try another one!";
                    return View(applicationUser);
                }

                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var result = userManager.Create(applicationUser, "Ante_007");
                if (!result.Succeeded) { throw new Exception(string.Join("\n", result.Errors)); }

                userManager.AddToRole(applicationUser.Id, "Teacher");
                return RedirectToAction("Index");

            }
            ViewBag.UserExist = "";
            return View(applicationUser);
        }

        public ActionResult ChangeProfile()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser applicationUser = db.Users.Find(userId);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

//          return View(applicationUser);

            var model = new ChangeProfileViewModels();
            model.GivenName = applicationUser.GivenName;
            model.FamilyName = applicationUser.FamilyName;
            model.ProfileImageRef = applicationUser.ProfileImageRef;
            model.PhoneNumber = applicationUser.PhoneNumber;

            return View(model);
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfile(ChangeProfileViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            ApplicationUser dbAU = db.Users.Find(userId);
            dbAU.GivenName = model.GivenName;
            dbAU.FamilyName = model.FamilyName;
            dbAU.ProfileImageRef = model.ProfileImageRef;
//          dbAU.Email = model.Email;
            dbAU.PhoneNumber = model.PhoneNumber;
            db.Entry(dbAU).State = EntityState.Modified;
            db.SaveChanges();

            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "TeacherCourses");
            }

            return RedirectToAction("Index");
        }


        // GET: ApplicationUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser applicationUser = db.Users.Find(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", applicationUser.CourseId);
            return View(applicationUser);
        }

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GivenName,FamilyName,ProfileImageRef,Email,PhoneNumber")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser dbAU = db.Users.Find(applicationUser.Id);
                dbAU.GivenName = applicationUser.GivenName;
                dbAU.FamilyName = applicationUser.FamilyName;
                dbAU.ProfileImageRef = applicationUser.ProfileImageRef;
                dbAU.Email = applicationUser.Email;
                dbAU.PhoneNumber = applicationUser.PhoneNumber;
                db.Entry(dbAU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", applicationUser.CourseId);
            return View(applicationUser);
        }


        // GET: ApplicationUser/Delete/5
        public ActionResult DeleteStudentFromCourse(string studnetId, int id)
        {
            if (studnetId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(studnetId);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = id;
            return View(applicationUser);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("DeleteStudentFromCourse")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudentFromCourseConfirmed(string studnetId, int id)
        {
            ApplicationUser applicationUser = db.Users.Find(studnetId);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Edit", "TeacherCourses", new { id = id });
        }


        // GET: ApplicationUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
