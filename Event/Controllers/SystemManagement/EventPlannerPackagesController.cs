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
        private readonly EventPlannerPackageSettingDataContext db = new EventPlannerPackageSettingDataContext();

        // GET: EventPlannerPackages
        public ActionResult Index()
        {
            var eventPlannerPackageSetting = db.EventPlannerPackages.Include(e => e.EventPlanner).Include(e => e.EventPlannerPackage);
            return View(eventPlannerPackageSetting.ToList());
        }

        // GET: EventPlannerPackages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            return View(eventPlannerPackageSetting);
        }

        // GET: EventPlannerPackages/Create
        public ActionResult Create()
        {
            ViewBag.PackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId", "PackageName");
            return View();
        }

        // POST: EventPlannerPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventPlannerPackageSettingId")] EventPlannerPackageSetting
                eventPlannerPackageSetting)
        {
            if (ModelState.IsValid)
            {
                var package = Session["package"] as EventPlannerPackage;
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                if (loggedinuser != null)
                {
                    if (loggedinuser.EventPlannerId != null)
                    eventPlannerPackageSetting.EventPlannerId = (long) loggedinuser.EventPlannerId;
                    eventPlannerPackageSetting.CreatedBy = loggedinuser.AppUserId;
                    eventPlannerPackageSetting.LastModifiedBy = loggedinuser.AppUserId;
                }
                eventPlannerPackageSetting.DateCreated = DateTime.Now;
                eventPlannerPackageSetting.DateLastModified = DateTime.Now;
                eventPlannerPackageSetting.Status = PackageStatusEnum.Active.ToString();
                eventPlannerPackageSetting.SubscribedEvent = 0;

                //package data
                if (package != null)
                {
                    eventPlannerPackageSetting.EventPlannerPackageId = package.EventPlannerPackageId;
                    eventPlannerPackageSetting.AllowedEvent = package.MaximumEvents;
                }
                db.EventPlannerPackages.Add(eventPlannerPackageSetting);
                db.SaveChanges();
                //display notification
                TempData["display"] = "You have successfully subscribed to the package!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.PackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId", "PackageName", eventPlannerPackageSetting.EventPlannerPackageId);
            return View(eventPlannerPackageSetting);
        }

        // GET: EventPlannerPackages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname",
                eventPlannerPackageSetting.EventPlannerId);
            ViewBag.PackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId", "PackageName", eventPlannerPackageSetting.EventPlannerPackageId);
            return View(eventPlannerPackageSetting);
        }

        // POST: EventPlannerPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "EventPlannerPackageSettingId,SubscribedEvent,AllowedEvent,PackageId,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
             )] EventPlannerPackageSetting eventPlannerPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPlannerPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname",
                eventPlannerPackage.EventPlannerId);
            ViewBag.PackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId", "PackageName", eventPlannerPackage.EventPlannerPackageId);
            return View(eventPlannerPackage);
        }

        // GET: EventPlannerPackages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackages.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            return View(eventPlannerPackageSetting);
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