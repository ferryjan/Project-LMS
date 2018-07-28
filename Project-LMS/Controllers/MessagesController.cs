using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Project_LMS.Models;
using Microsoft.AspNet.Identity;

namespace Project_LMS.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        [Authorize(Roles = "Student, Teacher")]
        public ActionResult MessageBox()
        {
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            return View(db.Messages.Where(m => m.FirstPersonLeft != user.Email && m.SecondPersonLeft != user.Email && m.isPublic == false && (m.SentFrom == user.Email || m.SentTo == user.Email)).OrderBy(m => m.MessageBoxNumber).ToList());
        }

        public ActionResult ShowPrivateChats(string id)
        {
            var listOfMsg = db.Messages.Where(m => m.MessageBoxNumber == id).OrderBy(m => m.SentDate).ToList();
            return PartialView("_showPrivateChats", listOfMsg);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        private string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();
            Random random = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }
            return builder.ToString();
        }

        // GET: Messages/Create
        public ActionResult SendMessage(string id)
        {            
            Message msg = new Message();
            msg.SentFrom = User.Identity.Name;
            var userId = User.Identity.GetUserId();
            ApplicationUser sfu = db.Users.Find(userId);
            msg.SentFromFullName = sfu.FullName;
            if (id != null && id != "")
            {
                ApplicationUser stu = db.Users.Find(id);
                msg.SentTo = stu.Email;
                msg.SentToFullName = stu.FullName;
            }
            else
            {
                msg.SentTo = "";
            }
            ViewBag.Err = "";
            return View(msg);
        }

        // POST: Messages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage([Bind(Include = "SentFrom,SentFromFullName,SentTo,Topic,Msg")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.isPublic = false;
                message.isRead = false;
                message.SentDate = DateTime.Now;
                message.MessageBoxNumber = RandomString(6);

                if (message.SentFrom == message.SentTo || db.Users.FirstOrDefault(u => u.Email == message.SentTo) == null)
                {
                    ViewBag.Err = "Error: The message receiver cannot be found in the database or you are sending message to yourself!";
                    return View(message);
                }
                message.SentToFullName = db.Users.FirstOrDefault(u => u.Email == message.SentTo).FullName;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("MessageBox");
            }
            ViewBag.Err = "";
            return View(message);
        }


        // GET: Messages/Create
        public ActionResult Reply(string id)
        {
            Message msgModel = new Message();
            var msg = db.Messages.FirstOrDefault(m => m.MessageBoxNumber == id);
            msgModel.isPublic = false;
            msgModel.isRead = false;
            msgModel.MessageBoxNumber = id;
            msgModel.Topic = msg.Topic;
            if (msg.SentFrom == User.Identity.Name)
            {
                msgModel.SentFrom = msg.SentFrom;
                msgModel.SentFromFullName = msg.SentFromFullName;
                msgModel.SentTo = msg.SentTo;
                msgModel.SentToFullName = msg.SentToFullName;
            }
            else
            {
                msgModel.SentFrom = msg.SentTo;
                msgModel.SentFromFullName = msg.SentToFullName;
                msgModel.SentTo = msg.SentFrom;
                msgModel.SentToFullName = msg.SentFromFullName;
            }
            if (db.Messages.FirstOrDefault(m => m.MessageBoxNumber == id && m.FirstPersonLeft != null ) != null)
            {
                ViewBag.HasLeft = "Yes";
            }
            else
            {
                ViewBag.HasLeft = "No";
            }
            

            var listOfMsg = db.Messages.Where(m => m.MessageBoxNumber == id && m.SentTo == User.Identity.Name && m.isRead == false).ToList();
            foreach (var m in listOfMsg)
            {
                m.isRead = true;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(msgModel);
        }

        // POST: Messages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "SentFrom,SentFromFullName,SentTo,SentToFullName,isPublic,isRead,MessageBoxNumber,Topic,Msg")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.SentDate = DateTime.Now;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Reply", new { id = message.MessageBoxNumber });
            }
            ViewBag.HasLeft = "No";
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SentFrom,SentTo,MessageBoxNumber,isRead,FirstPersonLeft,SecondPersonLeft,SentDate,Topic,Msg")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult LeaveChat(string id)
        {
            return View();
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("LeaveChat")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var listOfMsg = db.Messages.Where(m => m.MessageBoxNumber == id && (m.SentTo == User.Identity.Name || m.SentFrom == User.Identity.Name)).ToList();
            foreach (var m in listOfMsg)
            {
                if (m.SentTo == User.Identity.Name)
                {
                    m.isRead = true;
                }
                if (m.FirstPersonLeft == null)
                {
                    m.FirstPersonLeft = User.Identity.Name;
                }
                else
                {
                    m.SecondPersonLeft = User.Identity.Name;
                }
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("MessageBox");
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
