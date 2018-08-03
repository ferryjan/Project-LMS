using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_LMS.Models;

namespace Project_LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        [ChildActionOnly]
        [Authorize(Roles = "Teacher")]
        public ActionResult ShowCourseModules(int? id)
        {
            ViewBag.CourseId = id;
            ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            var modules = db.Modules.Where(i => i.CourseId == id).OrderBy(i => i.StartDate).OrderBy(i => i.EndDate);
            return PartialView(modules.ToList());
        }


        // GET: Modules/Details/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(int id, int? moduleId)
        {
            if (moduleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(moduleId);
            if (module == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = id;
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null){ return HttpNotFound(); }

            Module model = new Module();
            model.StartDate = course.StartDate;
            model.EndDate = course.EndDate;
            model.CourseId = course.CourseId;
            model.Course = course;

            ViewBag.DateNotValidMessage = "";
            return View(model);
        }

        // POST: Modules/Create
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "Name,StartDate,EndDate,Description")] Module module)
        {
            module.CourseId = id;
            Course course = db.Courses.Find(id);
            var courseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            var courseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            if (DateTime.Compare(courseStartDate, module.StartDate) > 0 || DateTime.Compare(courseEndDate, module.EndDate) < 0)
            {
                module.Course = course;
                ViewBag.DateNotValidMessage = ("Please make sure that the module start/end date is within the range of course start/end date!");
                return View(module);
            }
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Edit", "TeacherCourses", new { id });
            }
            module.Course = course;
            ViewBag.DateNotValidMessage = "";
            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }

            Course course = db.Courses.Find(module.CourseId);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.DateNotValidMessage = "";
            return View(module);
        }

        // POST: Modules/Edit/5
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ModuleId,Name,StartDate,EndDate,Description")] Module module)
        {
            module.CourseId = id;
            var courseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            var courseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            Course course = db.Courses.Find(id);
            if (DateTime.Compare(courseStartDate, module.StartDate) > 0 || DateTime.Compare(courseEndDate, module.EndDate) < 0)
            {
                ViewBag.CourseId = id;
                module.Course = course;
                ViewBag.DateNotValidMessage = ("Please make sure that the module start/end date is within the range of course start/end date!");
                return View(module);
            }
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "TeacherCourses", new { id = module.CourseId });
            }
            ViewBag.CourseId = id;
            module.Course = course;
            ViewBag.DateNotValidMessage = "";
            // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int id, int? moduleId, bool isVerified)
        {
            if (moduleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(moduleId);

            if (db.Documents.FirstOrDefault(d => d.ModuleId == moduleId) != null || db.Activities.FirstOrDefault(d => d.ModuleId == moduleId) != null)
            {
                ViewBag.IsEmpty = "No";
            }

            if (module == null)
            {
                return HttpNotFound();
            }

            if (isVerified)
            {
                ViewBag.VerifyComfirmed = "Yes";

            }
            else
            {
                ViewBag.VerifyComfirmed = "No";
            }
            ViewBag.CourseID = id;
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int moduleId, int id)
        {
            var listOfModDoc = db.Documents.Where(d => d.ModuleId == moduleId);
            if (listOfModDoc != null)
            {
                foreach (var doc in listOfModDoc)
                {
                    db.Documents.Remove(doc);
                }
            }

            var listOfActivity = db.Activities.Where(d => d.ModuleId == moduleId).ToList();
            if (listOfActivity != null)
            {
                foreach (var act in listOfActivity)
                {
                    var listOfActDoc = db.Documents.Where(d => d.ActivityId == act.ActivityId).ToList();
                    if (listOfActDoc != null)
                    {
                        foreach (var doc in listOfActDoc)
                        {
                            db.Documents.Remove(doc);
                        }
                    }
                    db.Activities.Remove(act);
                }
            }

            Module module = db.Modules.Find(moduleId);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Edit", "TeacherCourses", new { id });
        }

        // GET: Modules/MoveModule/5
        [Authorize(Roles = "Teacher")]
        public ActionResult MoveModule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return PartialView("_moveModule", module);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult MoveModule(int id)
        {
            // move logic here
            return RedirectToAction("Edit", "TeacherCourses", id);
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
