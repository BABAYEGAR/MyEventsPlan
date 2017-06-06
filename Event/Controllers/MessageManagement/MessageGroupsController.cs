using System;
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
    public class MessageGroupsController : Controller
    {
        private readonly MessageGroupDataContext db = new MessageGroupDataContext();

        // GET: MessageGroups
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            return View(db.MessageGroups.Where(n => n.CreatedBy == loggedinuser.AppUserId).ToList());
        }

        // GET: MessageGroups/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroup = db.MessageGroups.Find(id);
            if (messageGroup == null)
                return HttpNotFound();
            return View(messageGroup);
        }

        // GET: MessageGroups/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "MessageGroupId,Name")] MessageGroup messageGroup)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                messageGroup.DateCreated = DateTime.Now;
                messageGroup.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    messageGroup.CreatedBy = loggedinuser.AppUserId;
                    messageGroup.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                TempData["display"] = "You have successfully added a new message group!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                db.MessageGroups.Add(messageGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(messageGroup);
        }

        // GET: MessageGroups/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroup = db.MessageGroups.Find(id);
            if (messageGroup == null)
                return HttpNotFound();
            return View(messageGroup);
        }

        // POST: MessageGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "MessageGroupId,Name,CreatedBy,DateCreated")] MessageGroup messageGroup)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                messageGroup.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    messageGroup.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                TempData["display"] = "You have successfully modified the message group!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                db.MessageGroups.Add(messageGroup);
                db.Entry(messageGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messageGroup);
        }

        // GET: MessageGroups/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var messageGroup = db.MessageGroups.Find(id);
            if (messageGroup == null)
                return HttpNotFound();
            return View(messageGroup);
        }

        // POST: MessageGroups/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var messageGroup = db.MessageGroups.Find(id);
            db.MessageGroups.Remove(messageGroup);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the message group!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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