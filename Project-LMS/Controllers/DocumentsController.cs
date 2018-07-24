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

        // GET: Documents/Details/5
        public ActionResult ModuleDocumentDetails(int? id)
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

        // GET: Documents/Details/5
        public ActionResult ActivityDocumentDetails(int? id)
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
        public ActionResult CreateCourseDocument(int id)
        {

            ViewBag.CourseId = id;
            ViewBag.CourseName = db.Courses.FirstOrDefault(i => i.CourseId == id).CourseName;
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
                        db.Documents.Add(doc);
                        db.SaveChanges();
                        return RedirectToAction("Edit", "TeacherCourses", new { id = id });
                    }
                    else
                    {
                        ViewBag.NoFileSelectedMsg = "No file has been selected!";
                        ViewBag.CourseId = id;
                        return View(doc);
                    }
                }
            }
            ViewBag.CourseId = id;
            return View(doc);
        }

        // GET: Documents/Create
        public ActionResult CreateModuleDocument(int? id)
        {
            ViewBag.ModuleId = id;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateModuleDocument(Document doc, int id)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        doc.ApplicationUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                        doc.ModuleId = id;
                        doc.DocumentFileType = file.ContentType;
                        doc.UploadingTime = DateTime.Now;
                        doc.FileData = new byte[file.ContentLength];
                        doc.DocumentName = file.FileName;
                        file.InputStream.Read(doc.FileData, 0, file.ContentLength);
                        db.Documents.Add(doc);
                        db.SaveChanges();
                        return RedirectToAction("Edit", "Modules", new { id = id });
                    }
                    else
                    {
                        ViewBag.NoFileSelectedMsg = "No file has been selected!";
                        ViewBag.ModuleId = id;
                        return View(doc);
                    }
                    
                }
            }
            ViewBag.ModuleId = id;
            return View(doc);
        }

        // GET: Documents/Create
        public ActionResult CreateActivityDocument(int? id)
        {
            ViewBag.ActivityId = id;
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult CreateActivityDocument(Document doc, int id)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        doc.ApplicationUserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
                        doc.ActivityId = id;
                        doc.isHomework = false;
                        doc.DocumentFileType = file.ContentType;
                        doc.UploadingTime = DateTime.Now;
                        doc.FileData = new byte[file.ContentLength];
                        doc.DocumentName = file.FileName;
                        file.InputStream.Read(doc.FileData, 0, file.ContentLength);
                        db.Documents.Add(doc);
                        db.SaveChanges();
                        return RedirectToAction("Edit", "Activities", new { id });
                    }
                    else
                    {
                        ViewBag.NoFileSelectedMsg = "No file has been selected!";
                        ViewBag.ActivityId = id;
                        return View(doc);
                    }
                    
                }
            }
            ViewBag.ModuleId = id;
            return View(doc);
        }



        [HttpGet]
        public FileResult DownLoadFile(int? id)
        {
            ViewBag.CourseId = id;
            var FileById = db.Documents.Where(i => i.DocumentId == id).ToList().FirstOrDefault();
            return File(FileById.FileData, FileById.DocumentFileType, FileById.DocumentName);

        }


        public PartialViewResult FileDetails(int? id)
        {
            ViewBag.Id = id;
            var documents = db.Documents.Where(i => i.CourseId == id && !i.ModuleId.HasValue && !i.ActivityId.HasValue);
            return PartialView("FileDetails", documents.ToList());
        }

        public PartialViewResult ModuleFileDetails(int? moduleId)
        {
            ViewBag.Id = moduleId;
            var documents = db.Documents.Where(i => i.ModuleId == moduleId && !i.ActivityId.HasValue);
            return PartialView("ModuleFileDetails", documents.ToList());
        }

        public PartialViewResult ActivityFileDetails(int? activityId)
        {
            ViewBag.Id = activityId;
            var documents = db.Documents.Where(i => i.ActivityId == activityId && i.isHomework == false);
            return PartialView("_activityFileDetails", documents.ToList());
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
            var courseId = document.CourseId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Edit", "TeacherCourses", new { id = courseId });
        }

        // GET: Documents/Delete/5
        public ActionResult DeleteModuleDocument(int? id)
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
        [HttpPost, ActionName("DeleteModuleDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModuleDocumentConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            var moduleId = document.ModuleId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Edit", "Module", new { id = moduleId });
        }

        // GET: Documents/Delete/5
        public ActionResult DeleteActivityDocument(int? id)
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
        [HttpPost, ActionName("DeleteActivityDocument")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActivityDocumentConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            var activityId = document.ActivityId;
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Edit", "Activities", new { id = activityId });
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
