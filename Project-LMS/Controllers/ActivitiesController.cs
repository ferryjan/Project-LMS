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
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        public ActionResult Index(int? id)
        {
            //Fix do allow dev on activities before Modules are checked in.
            if (!id.HasValue) {id = db.Modules.FirstOrDefault().ModuleId;}

            ViewBag.ModuleName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Name;
            ViewBag.CourseName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Course.CourseName;
            ViewBag.ModuleId = id;

            var activities = db.Activities.Where(n => n.ModuleId == id ).Include(a => a.ActivityType);
            return View(activities.ToList());
        }

        // GET: Activities/Details/5
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
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create(int id)
        {
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type");
            ViewBag.ModuleId = id;
            ViewBag.ModuleName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Name;
            ViewBag.CourseName = db.Modules.FirstOrDefault(n => n.ModuleId == id).Course.CourseName;

            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityId,ActivityName,Start,End,Description,Deadline,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityId,ActivityName,Start,End,Description,Deadline,ModuleId,ActivityTypeId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityTypeId = new SelectList(db.ActivityTypes, "ActivityTypeId", "Type", activity.ActivityTypeId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
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
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
