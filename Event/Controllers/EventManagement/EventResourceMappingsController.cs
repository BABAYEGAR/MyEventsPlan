using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventResourceMappingsController : Controller
    {
        private readonly EventResourceMappingDataContext db = new EventResourceMappingDataContext();

        // GET: EventResourceMappings
        [SessionExpire]
        public ActionResult Index()
        {
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventResourceMapping =
                db.EventResourceMapping.Where(n => n.EventId == events.EventId).Include(e => e.Event)
                    .Include(e => e.Resource);
            ViewBag.ResourceId = new SelectList(
                db.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ResourceId", "Name");
            return View(eventResourceMapping.ToList());
        }


        // GET: EventResourceMappings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
                return HttpNotFound();
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventResourceMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "EventResourceMappingId,EventId,ResourceId,Quantity")]
            EventResourceMapping eventResourceMapping)
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
                var resourceId = eventResourceMapping.ResourceId;
                var resource = db.Resources.Find(resourceId);
                if (resource.Quantity > eventResourceMapping.Quantity)
                {
                    resource.Quantity = resource.Quantity - eventResourceMapping.Quantity;
                    resource.DateLastModified = DateTime.Now;
                    resource.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["display"] = "Your inventory does not have the quantity of resources required!";
                    TempData["notificationtype"] = NotificationType.Error.ToString();
                    return RedirectToAction("Index", new {eventId = eventResourceMapping.EventId});
                }
                db.Entry(resource).State = EntityState.Modified;
                db.EventResourceMapping.Add(eventResourceMapping);
                db.SaveChanges();
                TempData["display"] = "You have successfully allocated the resource(s) to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = eventResourceMapping.EventId});
            }
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(db.Event, "EventId", "Name", eventResourceMapping.EventId);
            ViewBag.ResourceId = new SelectList(
                db.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ResourceId", "Name",
                eventResourceMapping.ResourceId);
            return View(eventResourceMapping);
        }

        // POST: EventResourceMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "EventResourceMappingId,EventId,ResourceId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventResourceMapping eventResourceMapping)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventResourceMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventResourceMapping.LastModifiedBy = loggedinuser.AppUserId;
                    var resourceId = eventResourceMapping.ResourceId;
                    var resource = db.Resources.Find(resourceId);
                    if (resource.Quantity > eventResourceMapping.Quantity)
                    {
                        resource.Quantity = resource.Quantity - eventResourceMapping.Quantity;
                        resource.DateLastModified = DateTime.Now;
                        resource.LastModifiedBy = loggedinuser.AppUserId;
                    }
                    else
                    {
                        TempData["display"] = "Your inventory does not have the quantity of resources required!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index", new {eventId = eventResourceMapping.EventId});
                    }
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
                return RedirectToAction("Index", new {eventId = eventResourceMapping.EventId});
            }
            ViewBag.ResourceId = new SelectList(
                db.Resources.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ResourceId", "Name",
                eventResourceMapping.ResourceId);
            return View(eventResourceMapping);
        }

        // GET: EventResourceMappings/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventResourceMapping = db.EventResourceMapping.Find(id);
            if (eventResourceMapping == null)
                return HttpNotFound();
            return View(eventResourceMapping);
        }

        // POST: EventResourceMappings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventResourceMapping = db.EventResourceMapping.Find(id);

            var resourceId = eventResourceMapping.ResourceId;
            var resource = db.Resources.Find(resourceId);
            resource.Quantity = resource.Quantity + eventResourceMapping.Quantity;
            resource.DateLastModified = DateTime.Now;

            db.Entry(resource).State = EntityState.Modified;
            db.EventResourceMapping.Remove(eventResourceMapping);
            db.SaveChanges();
            TempData["display"] = "You have successfully deallocated the resources from the event!";
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