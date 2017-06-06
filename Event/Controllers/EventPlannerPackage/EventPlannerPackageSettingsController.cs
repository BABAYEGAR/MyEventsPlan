using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;

namespace MyEventPlan.Controllers.EventPlannerPackage
{
    public class EventPlannerPackageSettingsController : Controller
    {
        private readonly EventPlannerPackageSettingDataContext db = new EventPlannerPackageSettingDataContext();

        // GET: EventPlannerPackageSettings
        [SessionExpire]
        public ActionResult Index()
        {
            var eventPlannerPackageSettings = db.EventPlannerPackageSettings.Include(e => e.AppUser)
                .Include(e => e.EventPlanner).Include(e => e.EventPlannerPackage);
            return View(eventPlannerPackageSettings.ToList());
        }

        // GET: EventPlannerPackageSettings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackageSettings.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            return View(eventPlannerPackageSetting);
        }

        // GET: EventPlannerPackageSettings/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventPlannerPackageSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include =
                "EventPlannerPackageSettingId,Status,SubscribedEvent,AllowedEvent,EventPlannerPackageId,EventPlannerId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventPlannerPackageSetting eventPlannerPackageSetting)
        {
            if (ModelState.IsValid)
            {
                db.EventPlannerPackageSettings.Add(eventPlannerPackageSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventPlannerPackageSetting);
        }

        // GET: EventPlannerPackageSettings/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackageSettings.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname",
                eventPlannerPackageSetting.AppUserId);
            ViewBag.EventPlannerId = new SelectList(db.EventPlanner, "EventPlannerId", "Firstname",
                eventPlannerPackageSetting.EventPlannerId);
            ViewBag.EventPlannerPackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId",
                "PackageName", eventPlannerPackageSetting.EventPlannerPackageId);
            return View(eventPlannerPackageSetting);
        }

        // POST: EventPlannerPackageSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "EventPlannerPackageSettingId,Status,SubscribedEvent,AllowedEvent,EventPlannerPackageId,EventPlannerId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventPlannerPackageSetting eventPlannerPackageSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventPlannerPackageSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventPlannerPackageSetting);
        }

        // GET: EventPlannerPackageSettings/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = db.EventPlannerPackageSettings.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            return View(eventPlannerPackageSetting);
        }

        // POST: EventPlannerPackageSettings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerPackageSetting = db.EventPlannerPackageSettings.Find(id);
            db.EventPlannerPackageSettings.Remove(eventPlannerPackageSetting);
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