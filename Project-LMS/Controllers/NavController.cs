using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Project_LMS.Controllers
{
    [Authorize]
    public class NavController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Nav
        public ActionResult Menu()
        {
            var courseList = db.Courses.ToList();
            var moduleList = db.Modules.ToList();
            var activityList = db.Activities.ToList();
            var documentList = db.Documents.ToList();
            List<NaviItem> menuViewModel = new List<NaviItem>();
            NaviItem nav = new NaviItem() { Courses = courseList, Modules = moduleList, Activities = activityList, Documents = documentList };
            menuViewModel.Add(nav);
            return PartialView("_Navigation", menuViewModel);
        }

        [Authorize(Roles ="Student")]
        public ActionResult ShowStudentList()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser student = db.Users.Find(userId);
            var courseId = student.CourseId;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var studentRole = roleManager.FindByName("Student");
            var studentList = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).Where(t => t.CourseId == courseId && t.isActive == true).OrderBy(g => g.GivenName).ThenBy(f => f.FamilyName).ToList();
            List<StudentListNavItem> studentListViewModel = new List<StudentListNavItem>();
            StudentListNavItem sn = new StudentListNavItem() { Students = studentList };
            studentListViewModel.Add(sn);
            return PartialView("_StudentListNav", studentListViewModel);
        }
    }
}