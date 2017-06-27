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
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: EventPlannerPackageSettings
        [SessionExpire]
        public ActionResult Index()
        {
            var eventPlannerPackageSettings = _databaseConnection.EventPlannerPackageSettings.Include(e => e.AppUser)
                .Include(e => e.EventPlanner).Include(e => e.EventPlannerPackage);
            return View(eventPlannerPackageSettings.ToList());
        }

        // GET: EventPlannerPackageSettings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageSetting = _databaseConnection.EventPlannerPackageSettings.Find(id);
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
                _databaseConnection.EventPlannerPackageSettings.Add(eventPlannerPackageSetting);
                _databaseConnection.SaveChanges();
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
            var eventPlannerPackageSetting = _databaseConnection.EventPlannerPackageSettings.Find(id);
            if (eventPlannerPackageSetting == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(_databaseConnection.AppUsers, "AppUserId", "Firstname",
                eventPlannerPackageSetting.AppUserId);
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Firstname",
                eventPlannerPackageSetting.EventPlannerId);
            ViewBag.EventPlannerPackageId = new SelectList(_databaseConnection.EventPlannerPackages, "EventPlannerPackageId",
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
                _databaseConnection.Entry(eventPlannerPackageSetting).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
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
            var eventPlannerPackageSetting = _databaseConnection.EventPlannerPackageSettings.Find(id);
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
            var eventPlannerPackageSetting = _databaseConnection.EventPlannerPackageSettings.Find(id);
            _databaseConnection.EventPlannerPackageSettings.Remove(eventPlannerPackageSetting);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}