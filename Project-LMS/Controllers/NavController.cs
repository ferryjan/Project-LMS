﻿using Project_LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_LMS.Controllers
{
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
    }
}