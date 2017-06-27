using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MappingManagement
{
    public class EventVendorMappingsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: EventVendorMappings
        [SessionExpire]
        public ActionResult Index(long? id)
        {
            ViewBag.Event = id;
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var mappings =
                _databaseConnection.EventVendorMappings.Where(n => n.EventId == id && n.EventPlannerId == loggedinuser.EventPlannerId);
            var vedors =
                from a in _databaseConnection.Vendors
                join b in mappings on a.VendorId equals b.VendorId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors.Except(vedors), "VendorId", "Name");
            ViewBag.VendorServiceId = new SelectList(_databaseConnection.VendorServices, "VendorServiceId", "ServiceName");
            ViewBag.LocationId = new SelectList(_databaseConnection.Locations, "LocationId", "Name");
            var eventVendorMapping =
                _databaseConnection.EventVendorMappings.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId)
                    .Include(e => e.Event)
                    .Include(e => e.Vendor);
            return View(eventVendorMapping.Where(n => n.EventId == id).ToList());
        }

        // GET: EventVendorMappings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _databaseConnection.EventVendorMappings.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name");
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: EventVendorMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "EventVendorMappingId,EventId,VendorId")] EventVendorMapping eventVendorMapping,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventId = Convert.ToInt64(collectedValues["EventId"]);

            if (ModelState.IsValid)
            {
                eventVendorMapping.DateCreated = DateTime.Now;
                eventVendorMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventVendorMapping.CreatedBy = loggedinuser.AppUserId;
                    eventVendorMapping.LastModifiedBy = loggedinuser.AppUserId;
                    eventVendorMapping.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.EventVendorMappings.Add(eventVendorMapping);
                _databaseConnection.SaveChanges();
                TempData["mapping"] = "You have successfully assigned the vendor to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = eventId});
            }
            var mappings =
                _databaseConnection.EventVendorMappings.Where(
                    n => n.EventId == eventId && n.EventPlannerId == loggedinuser.EventPlannerId);
            var vedors =
                from a in _databaseConnection.Vendors
                join b in mappings on a.VendorId equals b.VendorId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors.Except(vedors), "VendorId", "Name");
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _databaseConnection.EventVendorMappings.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name", eventVendorMapping.EventId);
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", eventVendorMapping.VendorId);
            return View(eventVendorMapping);
        }

        // POST: EventVendorMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "EventVendorMappingId,EventId,VendorId,CreatedBy,DateCreated")] EventVendorMapping
                eventVendorMapping)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                eventVendorMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventVendorMapping.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(eventVendorMapping).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(_databaseConnection.Event, "EventId", "Name", eventVendorMapping.EventId);
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", eventVendorMapping.VendorId);
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _databaseConnection.EventVendorMappings.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            return View(eventVendorMapping);
        }

        // POST: EventVendorMappings/Delete/5
        [SessionExpire]
        public ActionResult RemoveVendorFromEvent(long? vendorId, long? eventId)
        {
            var eventVendorMapping =
                _databaseConnection.EventVendorMappings.SingleOrDefault(n => n.EventId == eventId && n.VendorId == vendorId);
            _databaseConnection.EventVendorMappings.Remove(eventVendorMapping);
            _databaseConnection.SaveChanges();
            return RedirectToAction("EventVendors", "Vendors");
        }

        // POST: EventVendorMappings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventVendorMapping = _databaseConnection.EventVendorMappings.Find(id);
            var eventId = eventVendorMapping.EventId;
            _databaseConnection.EventVendorMappings.Remove(eventVendorMapping);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index", new {id = eventId});
        }

        [SessionExpire]
        public ActionResult AddVendorToEvent(long? vendorId, long eventId)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var eventVendorMapping = new EventVendorMapping();
            if (ModelState.IsValid)
            {
                eventVendorMapping.DateCreated = DateTime.Now;
                eventVendorMapping.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventVendorMapping.CreatedBy = loggedinuser.AppUserId;
                    eventVendorMapping.LastModifiedBy = loggedinuser.AppUserId;
                    eventVendorMapping.EventPlannerId = loggedinuser.EventPlannerId;
                    eventVendorMapping.VendorId = vendorId;
                    eventVendorMapping.EventId = eventId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.EventVendorMappings.Add(eventVendorMapping);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully assigned the vendor to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("EventVendors", "Vendors");
            }
            return RedirectToAction("EventVendors", "Vendors");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}