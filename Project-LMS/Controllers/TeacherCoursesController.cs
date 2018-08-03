﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_LMS.Models;
using Project_LMS.ScheduleUI;
using Rotativa;

namespace Project_LMS.Controllers
{
    [Authorize]
    public class TeacherCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeacherCourses
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.EndDate.Date, DateTime.Now.Date) >= 0).OrderBy(c => c.StartDate).ToList());
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ShowUndergoingCourses()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.StartDate.Date, DateTime.Today) <= 0 && DateTime.Compare(i.EndDate.Date, DateTime.Today) >= 0).OrderBy(c => c.StartDate).ToList());
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ShowExpiredCourses()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.EndDate.Date, DateTime.Now.Date) < 0).OrderBy(c => c.StartDate).ToList());
        }

        // GET: TeacherCourses/Details/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: TeacherCourses/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }



        [Authorize(Roles = "Teacher")]
        public ActionResult PrintAllReport(int? id, string courseName)
        {
            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            return new ActionAsPdf("ShowPartialSchedule", new { id = id, courseName = courseName })
            {
                FileName = courseName + ".pdf",
                Cookies = cookieCollection
            };
        }
    

        // POST: TeacherCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,CourseName,StartDate,EndDate,CourseDescription")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: TeacherCourses/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: TeacherCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,StartDate,EndDate,CourseDescription")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: TeacherCourses/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id, bool isVerified)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);

            if (db.Modules.FirstOrDefault(d => d.CourseId == id) != null || db.Documents.FirstOrDefault(d => d.CourseId == id) != null)
            {
                ViewBag.IsEmpty = "No";
            }

            if (course == null)
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

            return View(course);
        }

        // POST: TeacherCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var listOfCourseDoc = db.Documents.Where(d => d.CourseId == id).ToList();
            if (listOfCourseDoc != null)
            {
                foreach (var doc in listOfCourseDoc)
                {
                    db.Documents.Remove(doc);
                }
            }

            var listOfStudent = db.Users.Where(d => d.CourseId == id).ToList();
            if (listOfStudent != null)
            {
                foreach (var student in listOfStudent)
                {
                    student.isActive = false;
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges(); ;
                }
            }

            var listOfModule = db.Modules.Where(m => m.CourseId == id).ToList();
            if (listOfModule != null)
            {
                foreach (var mod in listOfModule)
                {
                    var listOfModDoc = db.Documents.Where(d => d.ModuleId == mod.ModuleId).ToList();
                    if (listOfModDoc != null)
                    {
                        foreach (var doc in listOfModDoc)
                        {
                            db.Documents.Remove(doc);
                        }
                    }
                    var listOfActivity = db.Activities.Where(a => a.ModuleId == mod.ModuleId).ToList();
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
                    db.Modules.Remove(mod);
                }
            }


            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TeacherCourses/Schedule/6
        public ActionResult Schedule(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            Course listCourse = db.Courses.Find(id);
            if (listCourse == null) { return HttpNotFound(); }
            // create the schedule list
            var myScheduleManage = new ScheduleManage(db, listCourse);
            var mySchedule = myScheduleManage.RunAndGetList();
            return View(mySchedule);
        }

        public ActionResult ShowPartialSchedule(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            Course listCourse = db.Courses.Find(id);
            ViewBag.CourseId = id;
            if (listCourse == null) { return HttpNotFound(); }
            // create the schedule list
            var myScheduleManage = new ScheduleManage(db, listCourse);
            var mySchedule = myScheduleManage.RunAndGetList();
            return PartialView("_showPartialSchedule", mySchedule);
        }

        // GET: Modules/MoveCourse/5
        [Authorize(Roles = "Teacher")]
        public ActionResult MoveCourse(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

            Course course = db.Courses.Find(id);
            if (course == null) { return HttpNotFound(); }

            MoveCourseViewModel mcViewModel = new MoveCourseViewModel { Course = course, NewDate = course.StartDate };
            return PartialView("_moveCourse", mcViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult MoveCourse(MoveCourseViewModel mcViewModel)
        {
            Course course = db.Courses.Find(mcViewModel.Course.CourseId);
            var period = mcViewModel.NewDate - course.StartDate;
            var days = Convert.ToInt32(period.TotalDays);
            if (days == 0) { return RedirectToAction("Edit", "TeacherCourses", new { id = course.CourseId }); }

            course.StartDate = course.StartDate.AddDays(days);
            course.EndDate = course.EndDate.AddDays(days);
            foreach (var mod in course.CourseModules)
            {
                mod.StartDate = mod.StartDate.AddDays(days);
                mod.EndDate = mod.EndDate.AddDays(days);
                foreach (var act in mod.Activities)
                {
                    act.Start = act.Start.AddDays(days);
                    act.End = act.End.AddDays(days);
                }
            }

            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "TeacherCourses", null);
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
