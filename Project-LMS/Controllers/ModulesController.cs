﻿using System;
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
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Modules
        public ActionResult Index()
        {
            var modules = db.Modules.Include(m => m.Course);
            return View(modules.ToList());
        }

        [ChildActionOnly]
        public ActionResult ShowCourseModules(int? id)
        {
            ViewBag.CourseId = id;
            var modules = db.Modules.Where(i => i.CourseId == id);
            return PartialView(modules.ToList());
        }


        // GET: Modules/Details/5
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
        public ActionResult Create(int id)
        {
            ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            ViewBag.CourseId = id;
            ViewBag.DateNotValidMessage = "";
            return View();
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "Name,StartDate,EndDate,Description")] Module module)
        {
            module.CourseId = id;
            var courseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            var courseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            if (DateTime.Compare(courseStartDate, module.StartDate) > 0 || DateTime.Compare(courseEndDate, module.EndDate) < 0)
            {
                ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
                ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
                ViewBag.CourseId = id;
                ViewBag.DateNotValidMessage = ("Please make sure that the module start/end date is within the range of course start/end date!");
                return View(module);
            }
            if (ModelState.IsValid)
            {
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Edit", "TeacherCourses", new { id });
            }
            ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            ViewBag.CourseId = id;
            ViewBag.DateNotValidMessage = "";
            return View(module);
        }

        // GET: Modules/Edit/5
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
            ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == module.CourseId).StartDate.Date;
            ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == module.CourseId).EndDate.Date;
            ViewBag.CourseId = module.CourseId;
            ViewBag.DateNotValidMessage = "";
            // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ModuleId,Name,StartDate,EndDate,Description")] Module module)
        {
            module.CourseId = id;
            var courseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            var courseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            if (DateTime.Compare(courseStartDate, module.StartDate) > 0 || DateTime.Compare(courseEndDate, module.EndDate) < 0)
            {
                ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
                ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
                ViewBag.CourseId = id;
                ViewBag.DateNotValidMessage = ("Please make sure that the module start/end date is within the range of course start/end date!");
                return View(module);
            }
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "TeacherCourses", new { id = module.CourseId });
            }
            ViewBag.CourseStartDate = db.Courses.FirstOrDefault(c => c.CourseId == id).StartDate.Date;
            ViewBag.CourseEndDate = db.Courses.FirstOrDefault(c => c.CourseId == id).EndDate.Date;
            ViewBag.CourseId = id;
            ViewBag.DateNotValidMessage = "";
            // ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        public ActionResult Delete(int id, int? moduleId)
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

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int moduleId, int id)
        {
            Module module = db.Modules.Find(moduleId);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Edit", "TeacherCourses", new { id });
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
