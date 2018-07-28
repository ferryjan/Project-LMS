using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
            ViewBag.TimePeriod = course.StartDate.ToString() + " - " + course.EndDate.ToString();
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
