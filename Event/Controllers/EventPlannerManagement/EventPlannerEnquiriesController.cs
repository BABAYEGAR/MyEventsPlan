using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;

namespace MyEventPlan.Controllers.EventPlannerManagement
{
    public class EventPlannerEnquiriesController : Controller
    {
        private readonly EventPlannerEnquiryDataContext db = new EventPlannerEnquiryDataContext();

        // GET: EventPlannerEnquiries
        public ActionResult Index()
        {
            var eventPlannerEnquiries = db.EventPlannerEnquiries.Include(e => e.EventPlanner);
            return View(eventPlannerEnquiries.ToList());
        }

        // GET: EventPlannerEnquiries/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerEnquiry = db.EventPlannerEnquiries.Find(id);
            if (eventPlannerEnquiry == null)
                return HttpNotFound();
            return View(eventPlannerEnquiry);
        }

        // GET: EventPlannerEnquiries/Create
        public ActionResult Create()
        {
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Name");
            return View();
        }

        // POST: EventPlannerEnquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include =
                "EventPlannerEnquiryId,Name,Email,MobileNumber,EventDate,Note,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventPlannerEnquiry eventPlannerEnquiry)
        {
            if (ModelState.IsValid)
            {
                eventPlannerEnquiry.DateCreated = DateTime.Now;
                eventPlannerEnquiry.DateLastModified = DateTime.Now;
                db.EventPlannerEnquiries.Add(eventPlannerEnquiry);
                db.SaveChanges();
                return RedirectToAction("EventPlannerDetails", "EventPlanners",
                    new {id = eventPlannerEnquiry.EventPlannerId});
            }

            return RedirectToAction("EventPlannerDetails", "EventPlanners",
                new {id = eventPlannerEnquiry.EventPlannerId});
        }

        // GET: EventPlannerEnquiries/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerEnquiry = db.EventPlannerEnquiries.Find(id);
            if (eventPlannerEnquiry == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Name",
                eventPlannerEnquiry.EventPlannerId);
            return View(eventPlannerEnquiry);
        }

        // POST: EventPlannerEnquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include =
                "EventPlannerEnquiryId,Name,Email,MobileNumber,EventDate,Note,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventPlannerEnquiry eventPlannerEnquiry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPlannerEnquiry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(db.EventPlanners, "EventPlannerId", "Name",
                eventPlannerEnquiry.EventPlannerId);
            return View(eventPlannerEnquiry);
        }

        // GET: EventPlannerEnquiries/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerEnquiry = db.EventPlannerEnquiries.Find(id);
            if (eventPlannerEnquiry == null)
                return HttpNotFound();
            return View(eventPlannerEnquiry);
        }

        // POST: EventPlannerEnquiries/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerEnquiry = db.EventPlannerEnquiries.Find(id);
            db.EventPlannerEnquiries.Remove(eventPlannerEnquiry);
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