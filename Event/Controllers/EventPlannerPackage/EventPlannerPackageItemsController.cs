using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventPlannerPackage
{
    public class EventPlannerPackageItemsController : Controller
    {
        private readonly EventPlannerPackageItemDataContext db = new EventPlannerPackageItemDataContext();

        // GET: EventPlannerPackageItems
        [SessionExpire]
        public ActionResult Index(long id)
        {
            var eventPlannerPackageItems = db.EventPlannerPackageItems.Include(e => e.EventPlannerPackage)
                .Where(n => n.EventPlannerPackageId == id);
            ViewBag.packageId = id;
            return View(eventPlannerPackageItems.ToList());
        }

        // GET: EventPlannerPackageItems/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageItem = db.EventPlannerPackageItems.Find(id);
            if (eventPlannerPackageItem == null)
                return HttpNotFound();
            return View(eventPlannerPackageItem);
        }

        // GET: EventPlannerPackageItems/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.EventPlannerPackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId",
                "PackageName");
            return View();
        }

        // POST: EventPlannerPackageItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "EventPlannerPackageItemId,ItemName,Amount,EventPlannerPackageId")]
            EventPlannerPackageItem eventPlannerPackageItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                eventPlannerPackageItem.DateCreated = DateTime.Now;
                eventPlannerPackageItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventPlannerPackageItem.LastModifiedBy = loggedinuser.AppUserId;
                    eventPlannerPackageItem.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.EventPlannerPackageItems.Add(eventPlannerPackageItem);
                db.SaveChanges();

                var package = db.EventPlannerPackages.Find(eventPlannerPackageItem.EventPlannerPackageId);
                if (package != null) package.Amount = db.EventPlannerPackageItems.Sum(m => m.Amount);

                db.Entry(eventPlannerPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["display"] = "You have successfully added an item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = eventPlannerPackageItem.EventPlannerPackageId});
            }

            ViewBag.EventPlannerPackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId",
                "PackageName", eventPlannerPackageItem.EventPlannerPackageId);
            return View(eventPlannerPackageItem);
        }

        // GET: EventPlannerPackageItems/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageItem = db.EventPlannerPackageItems.Find(id);
            if (eventPlannerPackageItem == null)
                return HttpNotFound();
            ViewBag.EventPlannerPackageId = new SelectList(db.EventPlannerPackages, "EventPlannerPackageId",
                "PackageName", eventPlannerPackageItem.EventPlannerPackageId);
            return View(eventPlannerPackageItem);
        }

        // POST: EventPlannerPackageItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "EventPlannerPackageItemId,ItemName,Amount,EventPlannerPackageId,CreatedBy,DateCreated")]
            EventPlannerPackageItem eventPlannerPackageItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                eventPlannerPackageItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    eventPlannerPackageItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(eventPlannerPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                var package = db.EventPlannerPackages.Find(eventPlannerPackageItem.EventPlannerPackageId);
                if (package != null) package.Amount = db.EventPlannerPackageItems.Sum(m => m.Amount);

                db.Entry(eventPlannerPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["display"] = "You have successfully modified the item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = eventPlannerPackageItem.EventPlannerPackageId});
            }
            return View(eventPlannerPackageItem);
        }

        // GET: EventPlannerPackageItems/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerPackageItem = db.EventPlannerPackageItems.Find(id);
            if (eventPlannerPackageItem == null)
                return HttpNotFound();
            return View(eventPlannerPackageItem);
        }

        // POST: EventPlannerPackageItems/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerPackageItem = db.EventPlannerPackageItems.Find(id);
            db.EventPlannerPackageItems.Remove(eventPlannerPackageItem);
            db.SaveChanges();

            TempData["display"] = "You have successfully deleted the item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {id = eventPlannerPackageItem.EventPlannerPackageId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}