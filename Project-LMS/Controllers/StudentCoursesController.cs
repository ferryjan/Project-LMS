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

namespace Project_LMS.Controllers
{
    public class StudentCoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentStart/
        [Authorize(Roles = "Student")]
        public ActionResult StudentStart()
        {
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Find(userId);
            var course = db.Courses.First(u => u.CourseId == appUser.CourseId);
            ViewBag.CourseId = course.CourseId;
            ViewBag.CourseName = course.CourseName;
            ViewBag.TimePeriod = course.StartDate.ToString("yyyy-MM-dd") + " - " + course.EndDate.ToString("yyy-MM-dd");
            ViewBag.CourseDescription = course.CourseDescription;
            var modules = db.Modules.Where(i => i.CourseId == course.CourseId).ToList();
            return View(modules);
        }


        // GET: StudentCourses/Details/5
        [Authorize(Roles = "Student")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            var userId = User.Identity.GetUserId();
            var appUser = db.Users.Find(userId);
            if (applicationUser == null || appUser.CourseId != applicationUser.CourseId)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentUpcomingActivities(int? id)
        {
            ViewBag.Id = id;
            var todaysActivities = db.Activities.Where(i => i.Module.CourseId == id && (DateTime.Compare(i.Start, DateTime.Today) <= 0 && DateTime.Compare(i.End, DateTime.Today) >= 0));
            if (todaysActivities == null)
            {
                ViewBag.IsEmpty = "Yes";
            }
            else
            {
                ViewBag.IsEmpty = "No";
            }
            return PartialView("_studentUpcomingActivities", todaysActivities.ToList());
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentUpcomingHomeWork(int? id)
        {
            ViewBag.Id = id;
            DateTime week = DateTime.Today.AddDays(7);
            var tempList = db.Activities.Where(i => i.Module.CourseId == id && i.ActivityTypeId == 4 && DateTime.Compare(i.End, week) <= 0 && DateTime.Compare(i.End, DateTime.Today) > 0).ToList();
            var upcomingHomeWork = new List<Activity>();

            foreach (var activity in tempList)
            {
                if (db.Documents.FirstOrDefault(d => d.ActivityId == activity.ActivityId && d.ApplicationUser.Email == User.Identity.Name) == null)
                {
                    upcomingHomeWork.Add(activity);
                }
            }

            if (upcomingHomeWork.Count() == 0)
            {
                ViewBag.IsEmpty = "Yes";
            }
            else
            {
                ViewBag.IsEmpty = "No";
            }
            return PartialView("_upcomingHomeWork", upcomingHomeWork.ToList());
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentActivityFile(int? activityId)
        {
            ViewBag.Id = activityId;
            var documents = db.Documents.Where(i => i.ActivityId == activityId && i.isHomework == false);
            return PartialView("_studentActivityFile", documents.ToList());
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentHomeworkFile(int? activityId)
        {
            ViewBag.Id = activityId;
            var userId = User.Identity.GetUserId();
            var documents = db.Documents.Where(i => i.ActivityId == activityId && i.isHomework == true && i.ApplicationUserId == userId);
            return PartialView("_studentHomeworkFile", documents.ToList());
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentModuleFile(int? moduleId)
        {
            ViewBag.Id = moduleId;
            var documents = db.Documents.Where(i => i.ModuleId == moduleId && !i.ActivityId.HasValue);
            return PartialView("_studentModuleFile", documents.ToList());
        }

        [Authorize(Roles = "Student")]
        public PartialViewResult StudentCourseFile(int? id)
        {
            ViewBag.Id = id;
            var documents = db.Documents.Where(i => i.CourseId == id && !i.ModuleId.HasValue && !i.ActivityId.HasValue);
            return PartialView("_studentCourseFile", documents.ToList());
        }

        [Authorize(Roles = "Student")]
        public ActionResult StudentSchedule(int id)
        {
            ViewBag.CourseId = id;
            ViewBag.CourseName = db.Courses.FirstOrDefault(c => c.CourseId == id).CourseName;
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult ShowStudentListInMobileView()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser student = db.Users.Find(userId);
            var courseId = student.CourseId;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var studentRole = roleManager.FindByName("Student");
            var studentList = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).Where(t => t.CourseId == courseId && t.isActive == true).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
            return View(studentList);
        }

        public JsonResult GetEvents(int id)
        {
            IEnumerable<Event> eventsModelList = new List<Event>();

            var eventsList = db.Activities.Where(a => a.Module.CourseId == id).ToList();
            eventsModelList = eventsList.Select(x =>
                       new Event()
                       {
                           Subject = x.ActivityName,
                           Description = x.Description,
                           Start = x.Start,
                           End = x.End,
                           ThemeColor = x.Color,
                           IsFullDay = false
                       });
            return Json(eventsModelList, JsonRequestBehavior.AllowGet);
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
