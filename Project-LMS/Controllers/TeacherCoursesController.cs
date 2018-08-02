using System;
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
        public ActionResult PrintAllReport()
        {
            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            return new ActionAsPdf("Schedule", new { id = 1})
            {
                FileName = "Name.pdf",
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

        // UpdateModelName
        private ScheduleViewModels UpdateModelName(ScheduleViewModels model, string name)
        {
            if (model.Modul == "")
            {
                model.Modul = name;
            }
            else
            {
                model.ErrModul.Add(name);
            }

            return model;
        }

        // UpdatePMName
        private ScheduleViewModels UpdatePMName(ScheduleViewModels model, string name)
        {
            if (model.PM == "")
            {
                model.PM = name;
            }
            else
            {
                model.ErrPM.Add(name);
            }

            return model;
        }

        // UpdateAMName
        private ScheduleViewModels UpdateAMName(ScheduleViewModels model, string name)
        {
            if (model.AM == "")
            {
                model.AM = name;
            }
            else
            {
                model.ErrAM.Add(name);
            }

            return model;
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
            var courseStartDate = course.StartDate;
            var courseStoppDate = course.EndDate;
            var maxDates = 0;

            while (courseStoppDate >= courseStartDate)
            {
                courseStartDate = courseStartDate.AddDays(1);
                maxDates += 1;
            }

            ScheduleViewModels[] samling = new ScheduleViewModels[maxDates];
            courseStartDate = course.StartDate;

            var I = 0;
            while (courseStoppDate >= courseStartDate)
            {
                samling[I] = new ScheduleViewModels();
                samling[I].Date = courseStartDate.ToString("yyyy-MM-dd");
                samling[I].Day = courseStartDate.DayOfWeek.ToString();
                samling[I].Modul = "";
                samling[I].PM = ""; // + courseStartDate.DayOfYear.ToString();
                samling[I].AM = "";
                samling[I].ActuallDate = courseStartDate;
                samling[I].Year = courseStartDate.ToString("yyyy");
                samling[I].DayOfYear = courseStartDate.DayOfYear;
                samling[I].ErrModul = new List<string>();
                samling[I].ErrAM = new List<string>();
                samling[I].ErrPM = new List<string>();

                if (I % 2 == 0) { samling[I].bgColor = "LightSteelBlue"; }
                else samling[I].bgColor = "#ffffff";
                courseStartDate = courseStartDate.AddDays(1);
                I += 1;
            }

            var modules = db.Modules.Where(m => m.CourseId == course.CourseId).OrderBy(m => m.StartDate).OrderBy(m => m.EndDate);
            var moduleStartDate = course.StartDate;
            var moduleStoppDate = course.StartDate;
            foreach (var module in modules)
            {
                moduleStartDate = module.StartDate;
                moduleStoppDate = module.EndDate;

                // Lägg in data från Module för kursen
                while (moduleStoppDate >= moduleStartDate)
                {
                    var dag = moduleStartDate.DayOfYear;
                    var year = moduleStartDate.ToString("yyyy");
                    var modul = module.Name.ToString();
                    var found = false;
                    var counter = 0;

                    while (found == false)
                    {
                        if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                        {
                            samling[counter] = UpdateModelName(samling[counter], modul);
                            found = true;
                        }
                        counter += 1;
                        if (counter > maxDates) found = true; //Säkerhet
                    }

                    moduleStartDate = moduleStartDate.AddDays(1);
                }
            }


            //
            var activites = course.CourseModules.SelectMany(m => m.Activities).OrderBy(a => a.Start).OrderBy(a => a.End);
            courseStartDate = course.StartDate;
            var activiStartDate = course.StartDate;
            var activiStoppDate = course.StartDate;
            foreach (var activity in activites)
            {
                activiStartDate = activity.Start;
                activiStoppDate = activity.End;
                var firstTime = true;


                if (activity.ActivityType.Type == "Homework")
                {
                    activiStartDate = activity.End;

                    var dag = activiStartDate.DayOfYear;
                    var year = activiStartDate.ToString("yyyy");
                    var name = activity.ActivityName.ToString();
                    var found = false;
                    var counter = 0;

                    while (found == false)
                    {
                        if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                        {
                            samling[counter].Extern = "HomeWork";
                            found = true;
                        }

                        counter += 1;
                        if (counter > maxDates) found = true; //Säkerhet
                    }
                }
                else
                {

                    // Lägg in data för aktiviteten
                    while (activiStoppDate.Date >= activiStartDate.Date)
                    {
                        var dag = activiStartDate.DayOfYear;
                        var year = activiStartDate.ToString("yyyy");
                        var name = activity.ActivityName.ToString();
                        var found = false;
                        var counter = 0;

                        while (found == false)
                        {
                            if (year == samling[counter].Year && dag == samling[counter].DayOfYear)
                            {
                                if (activiStoppDate.Date == activiStartDate.Date)
                                {

                                    if (activiStartDate.Hour >= 13)
                                    {
                                        samling[counter] = UpdatePMName(samling[counter], name);
                                    }
                                    else if (activiStartDate.Hour >= 0 && activiStartDate.Hour < 13)
                                    {
                                        samling[counter] = UpdateAMName(samling[counter], name);

                                        if (activiStoppDate.Hour > 13 && activiStoppDate.Hour <= 24)
                                        {
                                            samling[counter] = UpdatePMName(samling[counter], name);
                                        }
                                    }
                                    else
                                    {
                                        samling[counter] = UpdatePMName(samling[counter], name);
                                    }

                                }
                                else if (activiStoppDate.Date > activiStartDate.Date)
                                {
                                    if (firstTime == true)
                                    {
                                        if (activiStartDate.Hour >= 13)
                                        {
                                            samling[counter] = UpdatePMName(samling[counter], name);
                                        }
                                        else if (activiStartDate.Hour >= 0 && activiStartDate.Hour < 13)
                                        {
                                            samling[counter] = UpdateAMName(samling[counter], name);

                                            if (activiStoppDate.Hour > 13 && activiStoppDate.Hour <= 24)
                                                samling[counter] = UpdatePMName(samling[counter], name);
                                        }
                                        else
                                        {
                                            samling[counter] = UpdatePMName(samling[counter], name);
                                        }

                                        firstTime = false;
                                    }
                                    else
                                    {
                                        samling[counter] = UpdateAMName(samling[counter], name);
                                        samling[counter] = UpdatePMName(samling[counter], name);
                                    }
                                }

                                if (activity.ActivityType.Type == "Lecture")
                                {
                                    samling[counter].Extern = "Extern";
                                }
                                else if (activity.ActivityType.Type == "Holiday")
                                {
                                    samling[counter].Extern = "Holiday";
                                }

                                found = true;
                            }

                            counter += 1;
                            if (counter > maxDates) found = true; //Säkerhet
                        }

                        activiStartDate = activiStartDate.AddDays(1);
                    }
                }
            }


            ScheduleHeadViewModels schedule = new ScheduleHeadViewModels();
            schedule.CourseId = course.CourseId;
            schedule.CourseName = course.CourseName;
            //
            schedule.myList = new List<ScheduleViewModels>();
            for (int i = 0; i < I; i++)
            {
                var model = new ScheduleViewModels();
                // Om lördag eller söndag, blanka fälten sam sätt backgrundsfärgen till Helgdag
                var bgColorHoliday = "#FFDAB9";
                var dayOfWeek = samling[i].ActuallDate.DayOfWeek.ToString();
                if (dayOfWeek == "Saturday" || dayOfWeek == "Sunday")
                {
                    samling[i].Modul = "";
                    samling[i].AM = "";
                    samling[i].PM = "";
                    samling[i].Extern = "";
                    samling[i].ErrModul = new List<string>();
                    samling[i].ErrAM = new List<string>();
                    samling[i].ErrPM = new List<string>();
                    samling[i].bgColor = bgColorHoliday;
                }

                var dag = samling[i].DayOfYear;
                var year = samling[i].Year;
                if (year == "2018" && (dag == 1 || dag == 6 || dag == 88 || dag == 89 | dag == 90 || dag == 91 || dag == 92 || dag == 121 || dag == 130 || dag == 140 || dag == 157 || dag == 174 || dag == 307 || dag == 359 || dag == 360))
                {
                    samling[i].Modul = "";
                    samling[i].AM = "";
                    samling[i].PM = "";
                    samling[i].Extern = "";
                    samling[i].ErrModul = new List<string>();
                    samling[i].ErrAM = new List<string>();
                    samling[i].ErrPM = new List<string>();
                    samling[i].bgColor = bgColorHoliday;
                }

                if (year == "2019" && (dag == 1 || dag == 6 || dag == 109 | dag == 111 || dag == 112 || dag == 150 || dag == 157 || dag == 160 || dag == 173 || dag == 306 || dag == 359 || dag == 360))
                {
                    samling[i].Modul = "";
                    samling[i].AM = "";
                    samling[i].PM = "";
                    samling[i].Extern = "";
                    samling[i].bgColor = bgColorHoliday;
                    samling[i].ErrModul = new List<string>();
                    samling[i].ErrAM = new List<string>();
                    samling[i].ErrPM = new List<string>();
                }

                if (samling[i].Extern == "Holiday")
                {
                    samling[i].Modul = "";
                    samling[i].AM = "";
                    samling[i].PM = "";
                    samling[i].Extern = "";
                    samling[i].bgColor = bgColorHoliday;
                    samling[i].ErrModul = new List<string>();
                    samling[i].ErrAM = new List<string>();
                    samling[i].ErrPM = new List<string>();
                }

                model = samling[i];
                schedule.myList.Add(model);
            }

            return View(schedule);
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
