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
    public class TeacherCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeacherCourses
        public ActionResult Index()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.EndDate, DateTime.Now) > 0).OrderBy(c => c.StartDate).ToList());
        }

        public ActionResult ShowUndergoingCourses()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.StartDate, DateTime.Now) < 0 && DateTime.Compare(i.EndDate, DateTime.Now) >= 0).OrderBy(c => c.StartDate).ToList());
        }

        public ActionResult ShowExpiredCourses()
        {
            return View(db.Courses.ToList().Where(i => DateTime.Compare(i.EndDate, DateTime.Now) <= 0).OrderBy(c => c.StartDate).ToList());
        }

        // GET: TeacherCourses/Details/5
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeacherCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
        public ActionResult Delete(int? id)
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

        // POST: TeacherCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TeacherCourses/Schedule/6
        public ActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Find(id);
            if (course == null) { return HttpNotFound(); }
            //
            List<ScheduleViewModels> myList = new List<ScheduleViewModels>();
            //
            var activites = course.CourseModules.SelectMany(m => m.Activities).OrderBy(a => a.Start);

            foreach (var activity in activites)
            {
                var model = new ScheduleViewModels();
                model.Date = activity.Start.ToString("yyyy-MM-dd");
                model.Day = activity.Start.ToString("ddddd");
                model.Modul = activity.ActivityId.ToString();
                model.PM = "Förmiddag";
                model.AM = "Eftermiddag";
                model.Extern =  "Nej";
                myList.Add(model);
            }


            return View(myList);
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
