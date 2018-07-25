using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
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
                var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == teacherRole.Id)).Where(u => u.isActive == true).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
                return View(list);
            }
            else
            {
                var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == teacherRole.Id)).Where(i => i.isActive == true && i.GivenName.ToLower().Contains(search.ToLower()) || i.FamilyName.ToLower().Contains(search.ToLower()) || i.Email.ToLower().Contains(search.ToLower())).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
                return View(list);
            }

        }

        public ActionResult StudentIndex(int id)
        {
            ViewBag.CourseId = id;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var studentRole = roleManager.FindByName("Student");
            var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).Where(t => t.CourseId == id && t.isActive == true).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
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

                if (db.Users.Any(u => u.UserName == applicationUser.Email) && db.Users.FirstOrDefault(u => u.UserName == applicationUser.Email).isActive == true)
                {
                    ViewBag.ErrMsg = "This email is existed in the database, try another one!";
                    ViewBag.CourseId = id;
                    return View(applicationUser);
                }
                if (!IsValidEmail(applicationUser.Email) || applicationUser.Email == null)
                {
                    ViewBag.ErrMsg = "This email address is not valid!";
                    ViewBag.CourseId = id;
                    return View(applicationUser);
                }

                if (db.Users.FirstOrDefault(u => u.UserName == applicationUser.Email).isActive == false)
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    var studentRole = roleManager.FindByName("Student");
                    var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).ToList();
                    if (list.FirstOrDefault(s => s.UserName == applicationUser.Email) == null) {
                        ViewBag.ErrMsg = "This email is existed in the database, try another one!";
                        ViewBag.CourseId = id;
                        return View(applicationUser);
                    }
                    else
                    {
                        ApplicationUser dbAU = db.Users.Find(list.FirstOrDefault(s => s.UserName == applicationUser.Email).Id);
                        dbAU.GivenName = applicationUser.GivenName;
                        dbAU.FamilyName = applicationUser.FamilyName;
                        dbAU.isActive = true;
                        db.Entry(dbAU).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Edit", "TeacherCourses", new { id = id });
                    }
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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

                if (db.Users.Any(u => u.UserName == applicationUser.Email) && db.Users.FirstOrDefault(u => u.UserName == applicationUser.Email).isActive == true)
                {
                    ViewBag.ErrMsg = "This email is existed in the database, try another one!";
                    return View(applicationUser);
                }
                if (!IsValidEmail(applicationUser.Email) || applicationUser.Email==null)
                {
                    ViewBag.ErrMsg = "This email address is not valid!";
                    return View(applicationUser);
                }

                if (db.Users.FirstOrDefault(u => u.UserName == applicationUser.Email).isActive == false)
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    var teacherRole = roleManager.FindByName("Teacher");
                    var list = db.Users.Where(x => x.Roles.Any(s => s.RoleId == teacherRole.Id)).ToList();
                    if (list.FirstOrDefault(s => s.UserName == applicationUser.Email) == null)
                    {
                        ViewBag.ErrMsg = "This email is existed in the database, try another one!";
                        return View(applicationUser);
                    }
                    else
                    {
                        ApplicationUser dbAU = db.Users.Find(list.FirstOrDefault(s => s.UserName == applicationUser.Email).Id);
                        dbAU.GivenName = applicationUser.GivenName;
                        dbAU.FamilyName = applicationUser.FamilyName;
                        dbAU.PhoneNumber = applicationUser.PhoneNumber;
                        dbAU.isActive = true;
                        db.Entry(dbAU).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
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
            model.Email = applicationUser.Email;
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
            var oldEmail = dbAU.Email;
            if (db.Users.Any(u => u.UserName == model.Email) && dbAU.UserName != model.Email)
            {
                ViewBag.Errmsg = "This email is existed in the database, try another one!";
                return View(model);
            }
            if (model.Email == "" || model.Email == null)
            {
                ViewBag.Errmsg = "You must type an valid email address here!";
                return View(model);
            }
            dbAU.Email = model.Email;
            dbAU.PhoneNumber = model.PhoneNumber;
            dbAU.UserName = model.Email;
            db.Entry(dbAU).State = EntityState.Modified;
            db.SaveChanges();
            if (oldEmail != model.Email)
            {
                Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                Session.Abandon();
                return RedirectToAction("RedirectToPage", "ApplicationUser");
            }
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "ApplicationUser");
            }

            return RedirectToAction("Index");
        }


        // GET: ApplicationUser/Edit/5
        public ActionResult Edit(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);

            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            //          return View(applicationUser);

            var model = new ChangeProfileViewModels();
            model.UserId = id;
            model.GivenName = applicationUser.GivenName;
            model.FamilyName = applicationUser.FamilyName;
            model.Email = applicationUser.Email;
            model.ProfileImageRef = applicationUser.ProfileImageRef;
            model.PhoneNumber = applicationUser.PhoneNumber;

            return View(model);
        }

       

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChangeProfileViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser dbAU = db.Users.Find(model.UserId);
            dbAU.GivenName = model.GivenName;
            dbAU.FamilyName = model.FamilyName;
            dbAU.ProfileImageRef = model.ProfileImageRef;
            var oldEmail = dbAU.Email;
            if (db.Users.Any(u => u.UserName == model.Email) && dbAU.UserName != model.Email)
            {
                ViewBag.Errmsg = "This email is existed in the database, try another one!";
                return View(model);
            }
            if (model.Email == "" || model.Email == null)
            {
                ViewBag.Errmsg = "You must type an valid email address here!";
                return View(model);
            }
            dbAU.Email = model.Email;
            dbAU.PhoneNumber = model.PhoneNumber;
            dbAU.UserName = model.Email;
            db.Entry(dbAU).State = EntityState.Modified;
            db.SaveChanges();
            if (oldEmail != model.Email)
            {
                Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                Session.Abandon();
                return RedirectToAction("RedirectToPage", "ApplicationUser");
            }

            return RedirectToAction("Index");
        }

        public ActionResult RedirectToPage()
        {
            return View();
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
            applicationUser.isActive = false;
            db.Entry(applicationUser).State = EntityState.Modified;
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
            //db.Users.Remove(applicationUser);
            applicationUser.isActive = false;
            db.Entry(applicationUser).State = EntityState.Modified;
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
