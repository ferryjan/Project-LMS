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
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.Activity).Include(d => d.ApplicationUser).Include(d => d.Course).Include(d => d.Module);
            return View(documents.ToList());
        }

        [ChildActionOnly]
        public ActionResult ShowCourseDocuments(int? courseId)
        {
            var documents = db.Documents.Where(i => i.CourseId == courseId && !i.ModuleId.HasValue && !i.ActivityId.HasValue);
            return PartialView(documents.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult CreateCourseDocument(int? id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateCourseDocument(Document doc, int id)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        doc.ApplicationUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                        doc.CourseId = id;
                        doc.DocumentFileType = file.ContentType;
                        doc.UploadingTime = DateTime.Now;
                        doc.FileData = new byte[file.ContentLength];
                        doc.DocumentName = file.FileName;
                        file.InputStream.Read(doc.FileData, 0, file.ContentLength);
                    }
                    db.Documents.Add(doc);
                    db.SaveChanges();
                    return RedirectToAction("Edit", "TeacherCourses", new { id = id });
                }
            }

            return View(doc);
        }

        [HttpGet]
        public FileResult DownLoadFile(int? id)
        {
            ViewBag.CourseId = id;
            var FileById = db.Documents.Where(i => i.DocumentId == id).ToList().FirstOrDefault();
            return File(FileById.FileData, FileById.DocumentFileType, FileById.DocumentName);

        }

        [HttpGet]
        public PartialViewResult FileDetails(int? id)
        {
            ViewBag.Id = id;
            var documents = db.Documents.Where(i => i.CourseId == id && !i.ModuleId.HasValue && !i.ActivityId.HasValue);
            return PartialView("FileDetails", documents.ToList());
        }


        // GET: Documents/Create
        public ActionResult Create()
        {
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "CourseName");
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "GivenName");
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentId,CourseName,CourseDescription,UploadingTime,DocumentRef,CourseId,ModuleId,ActivityId,ApplicationUserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "CourseName", document.ActivityId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "GivenName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "CourseName", document.ActivityId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "GivenName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", document.ModuleId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentId,CourseName,CourseDescription,UploadingTime,DocumentRef,CourseId,ModuleId,ActivityId,ApplicationUserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityId = new SelectList(db.Activities, "ActivityId", "CourseName", document.ActivityId);
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "GivenName", document.ApplicationUserId);
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", document.CourseId);
            ViewBag.ModuleId = new SelectList(db.Modules, "ModuleId", "CourseName", document.ModuleId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
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