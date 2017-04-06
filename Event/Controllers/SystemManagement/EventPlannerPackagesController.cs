using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.SystemManagement
{
    public class EventPlannerPackagesController : Controller
    {
        private readonly EventPlannerPackageDataContext db = new EventPlannerPackageDataContext();

        // GET: EventPlannerPackages
        public ActionResult Index()
        {
            var eventPlannerPackages = db.EventPlannerPackages.Include(e => e.EventPlanner).Include(e => e.Package);
            return View(eventPlannerPackages.ToList());
        }

        // GET: EventPlannerPackages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Create
        public ActionResult Create()
        {
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageName");
            return View();
        }

        // POST: EventPlannerPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventPlannerPackageId")] EventPlannerPackage
                eventPlannerPackage)
        {
            if (ModelState.IsValid)
            {
                var package = Session["package"] as Package;
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null)
                {
                    if (loggedinuser.EventPlannerId != null)
                    eventPlannerPackage.EventPlannerId = (long) loggedinuser.EventPlannerId;
                    eventPlannerPackage.CreatedBy = loggedinuser.AppUserId;
                    eventPlannerPackage.LastModifiedBy = loggedinuser.AppUserId;
                }
                eventPlannerPackage.DateCreated = DateTime.Now;
                eventPlannerPackage.DateLastModified = DateTime.Now;
                eventPlannerPackage.Status = PackageStatusEnum.Active.ToString();
                eventPlannerPackage.SubscribedEvent = 0;

                //package data
                if (package != null)
                {
                    eventPlannerPackage.PackageId = package.PackageId;
                    eventPlannerPackage.AllowedEvent = package.MaximumEvents;
                }
                db.EventPlannerPackages.Add(eventPlannerPackage);
                db.SaveChanges();
                //display notification
                TempData["display"] = "You have successfully subscribed to the package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageName", eventPlannerPackage.PackageId);
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname",
                eventPlannerPackage.EventPlannerId);
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageName", eventPlannerPackage.PackageId);
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "EventPlannerPackageId,SubscribedEvent,AllowedEvent,PackageId,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
             )] EventPlannerPackage eventPlannerPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPlannerPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname",
                eventPlannerPackage.EventPlannerId);
            ViewBag.PackageId = new SelectList(db.Packages, "PackageId", "PackageName", eventPlannerPackage.PackageId);
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackage == null)
                return HttpNotFound();
            return View(eventPlannerPackage);
        }

        // POST: EventPlannerPackages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerPackage = db.EventPlannerPackages.Find(id);
            db.EventPlannerPackages.Remove(eventPlannerPackage);
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