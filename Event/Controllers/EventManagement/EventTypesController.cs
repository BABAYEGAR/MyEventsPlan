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
using Event = Event.Data.Objects.Entities.Event;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventTypesController : Controller
    {
        private EventTypeDataContext db = new EventTypeDataContext();

        // GET: EventTypes
        public ActionResult Index()
        {
            return View(db.EventTypes.ToList());
        }

        // GET: EventTypes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType eventType = db.EventTypes.Find(id);
            if (eventType == null)
            {
                return HttpNotFound();
            }
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
            if (ModelState.IsValid)
            {
                eventType.DateCreated = DateTime.Now;
                eventType.DateLastModified = DateTime.Now;
                eventType.CreatedBy = null;
                eventType.LastModifiedBy = null;
                db.EventTypes.Add(eventType);
                db.SaveChanges();
                TempData["eventType"] = "You have successfully added an event type!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(eventType);
        }

        // GET: EventTypes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType eventType = db.EventTypes.Find(id);
            if (eventType == null)
            {
                return HttpNotFound();
            }
            return View(eventType);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventTypeId,Name,CreatedBy,DateCreated")] EventType eventType)
        {
            if (ModelState.IsValid)
            {
                eventType.DateLastModified = DateTime.Now;
                eventType.LastModifiedBy = null;
                db.Entry(eventType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["eventType"] = "You have successfully edit an event type!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(eventType);
        }

        // GET: EventTypes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventType eventType = db.EventTypes.Find(id);
            if (eventType == null)
            {
                return HttpNotFound();
            }
            return View(eventType);
        }

        // POST: EventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EventType eventType = db.EventTypes.Find(id);
            db.EventTypes.Remove(eventType);
            db.SaveChanges();
            TempData["eventType"] = "You have successfully deleted an event type!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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
