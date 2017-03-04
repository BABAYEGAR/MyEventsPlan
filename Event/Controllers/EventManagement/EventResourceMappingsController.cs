using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;
using System;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventResourceMappingsController : Controller
    {
        private EventResourceMappingDataContext db = new EventResourceMappingDataContext();

        // GET: EventResourceMappings
        public ActionResult Index(long? eventId)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventResourceMapping = db.EventResourceMapping.Where(n=>n.EventId == eventId).Include(e => e.Event).Include(e => e.Resource);
            ViewBag.eventId = eventId;
            ViewBag.ResourceId = new SelectList(db.Resources.Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId ), "ResourceId", "Name");
            return View(eventResourceMapping.ToList());
        }


        // GET: EventResourceMappings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventResourceMapping eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
            {
                return HttpNotFound();
            }
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventResourceMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventResourceMappingId,EventId,ResourceId")] EventResourceMapping eventResourceMapping)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventResourceMapping.DateCreated = DateTime.Now;
                eventResourceMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventResourceMapping.CreatedBy = loggedinuser.AppUserId;
                    eventResourceMapping.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.EventResourceMapping.Add(eventResourceMapping);
                db.SaveChanges();
                TempData["resourcemap"] = "You have successfully added the resource to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { eventId = eventResourceMapping.EventId });
            }
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Edit/5
        public ActionResult Edit(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventResourceMapping eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name", eventResourceMapping.EventId);
            ViewBag.ResourceId = new SelectList(db.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ResourceId", "Name", eventResourceMapping.ResourceId);
            return View(eventResourceMapping);
        }

        // POST: EventResourceMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventResourceMappingId,EventId,ResourceId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] EventResourceMapping eventResourceMapping)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventResourceMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventResourceMapping.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(eventResourceMapping).State = EntityState.Modified;
                db.SaveChanges();
                TempData["resourcemap"] = "You have successfully added the resource to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { eventId = eventResourceMapping.EventId });
            }
            ViewBag.ResourceId = new SelectList(db.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ResourceId", "Name", eventResourceMapping.ResourceId);
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventResourceMapping eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
            {
                return HttpNotFound();
            }
            return View(eventResourceMapping);
        }

        // POST: EventResourceMappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            EventResourceMapping eventResourceMapping = db.EventResourceMapping.Find(id);
            long eventId = (long)eventResourceMapping.EventId;
            db.EventResourceMapping.Remove(eventResourceMapping);
            db.SaveChanges();
            TempData["resourcemap"] = "You have successfully deleted the resource to the event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new { eventId = eventId });
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
