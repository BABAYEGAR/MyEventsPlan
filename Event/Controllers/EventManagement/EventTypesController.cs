using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventTypesController : Controller
    {
        private readonly EventTypeDataContext db = new EventTypeDataContext();

        // GET: EventTypes
        public ActionResult Index()
        {
            return View(db.EventTypes.ToList());
        }

        // GET: EventTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventType = db.EventTypes.Find(id);
            if (eventType == null)
                return HttpNotFound();
            return View(eventType);
        }

        // GET: EventTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventTypeId,Name")] EventType eventType)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventType.DateCreated = DateTime.Now;
                eventType.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventType.CreatedBy = loggedinuser.AppUserId;
                    eventType.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.EventTypes.Add(eventType);
                db.SaveChanges();
                TempData["display"] = "You have successfully added an event type!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(eventType);
        }

        // GET: EventTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventType = db.EventTypes.Find(id);
            if (eventType == null)
                return HttpNotFound();
            return View(eventType);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventTypeId,Name,CreatedBy,DateCreated")] EventType eventType)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventType.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventType.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(eventType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully edit an event type!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(eventType);
        }

        // GET: EventTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventType = db.EventTypes.Find(id);
            if (eventType == null)
                return HttpNotFound();
            return View(eventType);
        }

        // POST: EventTypes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventType = db.EventTypes.Find(id);
            db.EventTypes.Remove(eventType);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted an event type!";
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