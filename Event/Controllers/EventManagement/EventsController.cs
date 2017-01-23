using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Services;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventsController : Controller
    {
        private readonly EventDataContext db = new EventDataContext();

        // GET: Events
        public ActionResult Index()
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            var events = db.Event.Include(n => n.EventType).Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            return View(events.ToList());
        }

        // GET: Events
        public JsonResult GetMyEvents()
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            var events = db.Event.Include(n => n.EventType).Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            return Json(events, JsonRequestBehavior.AllowGet);
        }
        // GET: Events/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,EndDate,EndTime")] Event.Data.Objects.Entities.Event @event)
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                if (loggedinuser != null)
                {
                    @event.CreatedBy = loggedinuser.AppUserId;
                    @event.DateCreated = DateTime.Now;
                    @event.DateLastModified = DateTime.Now;
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Event.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "EventId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,EndDate,EndTime,CreatedBy,DateCreated"
             )] Event.Data.Objects.Entities.Event @event)
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                @event.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var @event = db.Event.Find(id);
            db.Event.Remove(@event);
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