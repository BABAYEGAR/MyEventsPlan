using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MessageManagement
{
    public class MessagesController : Controller
    {
        private readonly MessageDataContext db = new MessageDataContext();
        private readonly NotificationDataContext dbc = new NotificationDataContext();
        private readonly EventDataContext dbd = new EventDataContext();

        // GET: Messages
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var messages =
                db.Messages.Where(n => n.AppUserId == loggedinuser.AppUserId)
                    .Include(m => m.AppUser)
                    .Include(m => m.MessageGroup);
            var allUsers =
                db.AppUsers.Where(
                    n =>
                        n.AppUserId != loggedinuser.AppUserId && n.AppUserId != 4 && n.ClientId != null &&
                        n.VendorId != null);
            var eventPlannerEvents = dbd.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            var eventPlannerEventsMapping =
                dbd.EventVendorMappings.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            var users = new List<AppUser>();
            foreach (var item in allUsers)
            {
                if (item.ClientId != null)
                {
                    var client = dbd.Clients.Find(item.ClientId);
                    if (eventPlannerEvents.Any(n => n.EventId == client.EventId))
                        users.Add(item);
                }
                if (item.VendorId != null)
                    if (eventPlannerEventsMapping.Any(n => n.VendorId == item.VendorId))
                        users.Add(item);
            }
            ViewBag.AppUserId = new SelectList(users,
                "AppUserId", "DisplayName");
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name");
            return View(messages.ToList());
        }

        // GET: Messages
        [SessionExpire]
        public ActionResult SentMessages()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var messages =
                db.Messages.Where(n => n.Sender == loggedinuser.AppUserId)
                    .Include(m => m.AppUser)
                    .Include(m => m.MessageGroup);
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.AppUserId != loggedinuser.AppUserId),
                "AppUserId", "DisplayName");
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name");
            return View(messages.ToList());
        }

        // GET: Messages/Details/5
        [SessionExpire]
        public ActionResult Details(long? id, long? notificationId)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var message = db.Messages.Find(id);
            var notification = dbc.Notifications.Find(notificationId);
            notification.Read = true;
            notification.DateLastModified = DateTime.Now;
            if (loggedinuser != null) notification.LastModifiedBy = loggedinuser.AppUserId;
            dbc.Entry(notification).State = EntityState.Modified;
            dbc.SaveChanges();
            return View(message);
        }

        // GET: Messages/Create
        [SessionExpire]
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
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "MessageId,Subject,Body,AttachedFile,AppUserId,MessageGroupId")] Message message)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (message.AppUserId != null)
                {
                    message.DateCreated = DateTime.Now;
                    message.DateLastModified = DateTime.Now;
                    if (loggedinuser != null)
                    {
                        message.LastModifiedBy = loggedinuser.AppUserId;
                        message.CreatedBy = loggedinuser.AppUserId;
                        message.Sender = loggedinuser.AppUserId;
                        message.Read = false;
                        db.Messages.Add(message);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["login"] = "Your session has expired, Login again!";
                        TempData["notificationtype"] = NotificationType.Info.ToString();
                        return RedirectToAction("Login", "Account");
                    }
                }
                if (message.MessageGroupId != null)
                {
                    var group = db.MessageGroupMembers.Where(n => n.MessageGroupId == message.MessageGroupId);
                    foreach (var item in group.ToList())
                    {
                        message.AppUserId = item.AppUserId;
                        if (loggedinuser != null)
                        {
                            message.LastModifiedBy = loggedinuser.AppUserId;
                            message.CreatedBy = loggedinuser.AppUserId;
                            message.Sender = loggedinuser.AppUserId;
                        }
                        message.DateCreated = DateTime.Now;
                        message.DateLastModified = DateTime.Now;
                        message.Read = false;
                        db.Messages.Add(message);
                        db.SaveChanges();
                        message.AppUserId = null;
                    }
                }

                if (message.MessageGroupId != null)
                {
                    var group = db.MessageGroupMembers.Where(n => n.MessageGroupId == message.MessageGroupId);
                    foreach (var item in group.ToList())
                    {
                        var notification = new Notification();
                        notification.AppUserId = item.AppUserId;
                        if (loggedinuser != null)
                        {
                            notification.Message = "Platform message from " + loggedinuser.DisplayName + "!";
                            notification.NotificationKey = message.MessageId;
                            notification.DateCreated = DateTime.Now;
                            notification.CreatedBy = loggedinuser.AppUserId;
                            notification.Read = false;
                            notification.DateLastModified = DateTime.Now;
                            notification.LastModifiedBy = loggedinuser.AppUserId;
                        }
                        notification.NotificationType = AppNotificationType.Message.ToString();
                        dbc.Notifications.Add(notification);
                        dbc.SaveChanges();
                    }
                }

                if (message.AppUserId != null)
                {
                    var notification = new Notification();
                    notification.AppUserId = (long) message.AppUserId;
                    if (loggedinuser != null)
                    {
                        notification.Message = "Platform message from " + loggedinuser.DisplayName + "!";
                        notification.NotificationKey = message.MessageId;
                        notification.DateCreated = DateTime.Now;
                        notification.CreatedBy = loggedinuser.AppUserId;
                        notification.Read = false;
                        notification.DateLastModified = DateTime.Now;
                        notification.LastModifiedBy = loggedinuser.AppUserId;
                    }
                    notification.NotificationType = AppNotificationType.Message.ToString();
                    dbc.Notifications.Add(notification);
                    dbc.SaveChanges();
                }
                TempData["display"] = "Your platform message has been sent successfully!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", message.AppUserId);
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name", message.MessageGroupId);
            return View(message);
        }

        // GET: Messages/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var message = db.Messages.Find(id);
            if (message == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", message.AppUserId);
            ViewBag.MessageGroupId = new SelectList(db.MessageGroups, "MessageGroupId", "Name", message.MessageGroupId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "MessageId,Subject,AttachedFile,Sender,AppUserId,MessageGroupId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
            )] Message message)
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
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var message = db.Messages.Find(id);
            if (message == null)
                return HttpNotFound();
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}