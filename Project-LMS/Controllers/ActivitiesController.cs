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
    [Authorize]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Teacher")]
        // GET: Activities
        public ActionResult Index(int? id)
        {
            //Fix do allow dev on activities before Modules are checked in.
            if (!id.HasValue) { id = db.Modules.FirstOrDefault().ModuleId; }

            ViewBag.ModuleName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Name;
            ViewBag.CourseName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Course.CourseName;
            ViewBag.ModuleId = id;

            var activities = db.Activities.Where(n => n.ModuleId == id).Include(a => a.ActivityType);
            return View(activities.OrderBy(s => s.Start).ToList());
        }

        [ChildActionOnly]
        [Authorize(Roles = "Teacher")]
        public ActionResult ShowActivities(int? id)
        {
            ViewBag.ModuleId = id;
            var activities = db.Activities.Where(i => i.ModuleId == id).OrderBy(o => o.Start);
            return PartialView("_Index", activities.ToList());
        }

        // GET: Activities/Details/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModuleName = activity.Module.Name;
            ViewBag.CourseName = activity.Module.Course.CourseName;
            return View(activity);
        }


        // GET: Activities/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int id)
        {
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type");
            // create a new model
            ActivityViewModels activityModel = new ActivityViewModels();
            Module module = db.Modules.Find(id);
            if (module == null) { return HttpNotFound(); }

            activityModel.ModuleId = id;
            activityModel.ModuleName = module.Name;
            activityModel.ModuleStartDate = module.StartDate;
            activityModel.ModuleEndDate = module.EndDate;
            //
            Course course = db.Courses.Find(module.CourseId);
            if (course == null) { return HttpNotFound(); }
            activityModel.CourseName = course.CourseName;
            activityModel.CourseStartDate = course.StartDate;
            activityModel.CourseEndDate = course.EndDate;
            //
            activityModel.Start = db.Modules.FirstOrDefault(n => n.ModuleId == id).StartDate;
            activityModel.End = db.Modules.FirstOrDefault(n => n.ModuleId == id).EndDate;

            return View(activityModel);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, ActivityViewModels activityModel)
        {
            if (activityModel != null) { activityModel.ModuleId = id; }
            Activity activity = new Activity();

            if (ModelState.IsValid)
            {
                activity.ModuleId = activityModel.ModuleId;
                activity.ActivityName = activityModel.ActivityName;
                activity.ActivityTypeId = activityModel.ActivityTypeId;
                activity.Description = activityModel.Description;
                activity.Start = activityModel.Start;
                activity.End = activityModel.End;

                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Edit", "Modules", new { id = activity.ModuleId });
            }


            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            Module module = db.Modules.Find(id);
            if (module == null) { return HttpNotFound(); }

            activityModel.ModuleId = id;
            activityModel.ModuleName = module.Name;
            activityModel.ModuleStartDate = module.StartDate;
            activityModel.ModuleEndDate = module.EndDate;
            //
            Course course = db.Courses.Find(module.CourseId);
            if (course == null) { return HttpNotFound(); }
            activityModel.CourseName = course.CourseName;
            activityModel.CourseStartDate = course.StartDate;
            activityModel.CourseEndDate = course.EndDate;
            //
            activityModel.Start = db.Modules.FirstOrDefault(n => n.ModuleId == id).StartDate;
            activityModel.End = db.Modules.FirstOrDefault(n => n.ModuleId == id).EndDate;

            return View(activityModel);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            Activity activity = db.Activities.Find(id);
            if (activity == null) { return HttpNotFound(); }

            // create a new model
            ActivityViewModels activityModel = new ActivityViewModels();
            activityModel.ActivityId = activity.ActivityId;
            activityModel.ActivityName = activity.ActivityName;
            activityModel.ActivityTypeId = activity.ActivityTypeId;
            activityModel.ActivityType = activity.ActivityType;
            activityModel.Description = activity.Description;
            activityModel.Start = activity.Start;
            activityModel.End = activity.End;
            //
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            //
            Module module = db.Modules.Find(activity.ModuleId);
            if (module == null) { return HttpNotFound(); }
            activityModel.ModuleId = activity.ModuleId;
            activityModel.ModuleName = module.Name;
            activityModel.ModuleStartDate = module.StartDate;
            activityModel.ModuleEndDate = module.EndDate;
            //
            Course course = db.Courses.Find(module.CourseId);
            if (course == null) { return HttpNotFound(); }
            activityModel.CourseName = course.CourseName;
            activityModel.CourseStartDate = course.StartDate;
            activityModel.CourseEndDate = course.EndDate;
            //
            return View(activityModel);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.ModuleId = id;
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Modules", new { id });
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id, bool isVerified)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (db.Documents.FirstOrDefault(d => d.ActivityId == id) != null) {
                ViewBag.IsEmpty = "No";
            }
            if (activity == null)
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
                return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var listOfDoc = db.Documents.Where(d => d.ActivityId == id).ToList();
            if (listOfDoc != null)
            {
                foreach (var doc in listOfDoc)
                {
                    db.Documents.Remove(doc);
                }
            }
            
            Activity activity = db.Activities.Find(id);
            var ModuleId = activity.ModuleId;
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Edit", "Modules", new { id = ModuleId });
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult ShowHomeworkList(int id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var studentRole = roleManager.FindByName("Student");
            var courseId = db.Activities.FirstOrDefault(a => a.ActivityId == id).Module.CourseId;
            var studentList = db.Users.Where(x => x.Roles.Any(s => s.RoleId == studentRole.Id)).Where(s => s.CourseId == courseId).OrderBy(i => i.GivenName).ThenBy(i => i.FamilyName).ToList();
            var documentList = db.Documents.Where(d => d.ActivityId == id && d.isHomework == true).ToList();

            List<HomeworkViewModels> homeworkVM = new List<HomeworkViewModels>();
            HomeworkViewModels hvm = new HomeworkViewModels() { Documents = documentList, Students = studentList };
            homeworkVM.Add(hvm);
            return PartialView("_homeworkList", homeworkVM);
        }

        //public PartialViewResult ActivityFileDetails(int? activityId)
        //{
        //    ViewBag.Id = activityId;
        //    var documents = db.Documents.Where(i => i.ActivityId == activityId && i.isHomework == false);
        //    return PartialView("_activityFileDetails", documents.ToList());
        //}

        [Authorize(Roles = "Teacher")]
        public PartialViewResult ActivityFileDetails(int? activityId)
        {
            ViewBag.Id = activityId;
            var documents = db.Documents.Where(i => i.ActivityId == activityId && i.isHomework == false);
            return PartialView("_activityFileDetails", documents.ToList());
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
