using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MessageManagement
{
    public class MessagesController : Controller
    {
        private MessageDataContext db = new MessageDataContext();

        // GET: Messages
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var messages = db.Messages.Where(n=>n.AppUserId == loggedinuser.AppUserId ).Include(m => m.AppUser).Include(m => m.MessageGroup);
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n=>n.AppUserId != loggedinuser.AppUserId), "AppUserId", "DisplayName");
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name");
            return View(messages.ToList());
        }
        // GET: Messages
        public ActionResult SentMessages()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var messages = db.Messages.Where(n => n.Sender == loggedinuser.AppUserId).Include(m => m.AppUser).Include(m => m.MessageGroup);
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.AppUserId != loggedinuser.AppUserId), "AppUserId", "DisplayName");
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name");
            return View(messages.ToList());
        }
        // GET: Messages/Details/5
        public ActionResult Details(long? id)
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

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname");
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,Subject,Body,AttachedFile,AppUserId,MessageGroupId")] Message message)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                message.DateCreated = DateTime.Now;
                message.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    message.LastModifiedBy = loggedinuser.AppUserId;
                    message.CreatedBy = loggedinuser.AppUserId;
                    message.Sender = loggedinuser.AppUserId;
                    message.Read = false;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", message.AppUserId);
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name", message.MessageGroupId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(long? id)
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
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", message.AppUserId);
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name", message.MessageGroupId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,Subject,AttachedFile,Sender,AppUserId,MessageGroupId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", message.AppUserId);
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name", message.MessageGroupId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(long? id)
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

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
