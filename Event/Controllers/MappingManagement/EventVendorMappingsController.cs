using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.MappingManagement
{
    public class EventVendorMappingsController : Controller
    {
        private readonly EventVendorMappingDataContext _db = new EventVendorMappingDataContext();

        // GET: EventVendorMappings
        public ActionResult Index(long? id)
        {
            ViewBag.Event = id;
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var mappings =
                _db.EventVendorMapping.Where(n => (n.EventId == id) && (n.EventPlannerId == loggedinuser.EventPlannerId));
            var vedors =
                from a in _db.Vendors
                join b in mappings on a.VendorId equals b.VendorId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.VendorId = new SelectList(_db.Vendors.Except(vedors), "VendorId", "Name");
            var eventVendorMapping =
                _db.EventVendorMapping.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId)
                    .Include(e => e.Event)
                    .Include(e => e.Vendor);
            return View(eventVendorMapping.Where(n => n.EventId == id).ToList());
        }

        // GET: EventVendorMappings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _db.EventVendorMapping.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name");
            ViewBag.VendorId = new SelectList(_db.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: EventVendorMappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _db.EventVendorMapping.Add(eventVendorMapping);
                _db.SaveChanges();
                TempData["mapping"] = "You have successfully assigned the vendor to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = eventId});
            }
            var mappings =
                _db.EventVendorMapping.Where(
                    n => (n.EventId == eventId) && (n.EventPlannerId == loggedinuser.EventPlannerId));
            var vedors =
                from a in _db.Vendors
                join b in mappings on a.VendorId equals b.VendorId
                where a.EventPlannerId == loggedinuser.EventPlannerId
                select a;

            ViewBag.VendorId = new SelectList(_db.Vendors.Except(vedors), "VendorId", "Name");
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _db.EventVendorMapping.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name", eventVendorMapping.EventId);
            ViewBag.VendorId = new SelectList(_db.Vendors, "VendorId", "Name", eventVendorMapping.VendorId);
            return View(eventVendorMapping);
        }

        // POST: EventVendorMappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _db.Entry(eventVendorMapping).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name", eventVendorMapping.EventId);
            ViewBag.VendorId = new SelectList(_db.Vendors, "VendorId", "Name", eventVendorMapping.VendorId);
            return View(eventVendorMapping);
        }

        // GET: EventVendorMappings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventVendorMapping = _db.EventVendorMapping.Find(id);
            if (eventVendorMapping == null)
                return HttpNotFound();
            return View(eventVendorMapping);
        }

        // POST: EventVendorMappings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventVendorMapping = _db.EventVendorMapping.Find(id);
            var eventId = eventVendorMapping.EventId;
            _db.EventVendorMapping.Remove(eventVendorMapping);
            _db.SaveChanges();
            return RedirectToAction("Index", new {id = eventId});
        }

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
                _db.EventVendorMapping.Add(eventVendorMapping);
                _db.SaveChanges();
                TempData["display"] = "You have successfully assigned the vendor to the event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("ListOfVendors", "Vendors");
            }
            return View(eventVendorMapping);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}