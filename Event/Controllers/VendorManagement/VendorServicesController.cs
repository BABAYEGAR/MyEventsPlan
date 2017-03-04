using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorServicesController : Controller
    {
        private readonly VendorServiceDataContext db = new VendorServiceDataContext();

        // GET: VendorServices
        public ActionResult Index()
        {
            return View(db.VendorService.ToList());
        }

        // GET: VendorServices/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = db.VendorService.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // GET: VendorServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorServiceId,ServiceName,Scale")] VendorService vendorService,
            FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorService.DateCreated = DateTime.Now;
                vendorService.DateLastModified = DateTime.Now;
                vendorService.Scale = typeof(VendorScale).GetEnumName(int.Parse(collectedValues["Scale"]));
                if (loggedinuser != null)
                {
                    vendorService.LastModifiedBy = loggedinuser.AppUserId;
                    vendorService.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.VendorService.Add(vendorService);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a vendor service!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(vendorService);
        }

        // GET: VendorServices/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = db.VendorService.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // POST: VendorServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "VendorServiceId,ServiceName,CreatedBy,DateCreated")] VendorService vendorService,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                vendorService.DateLastModified = DateTime.Now;
                vendorService.Scale = typeof(VendorScale).GetEnumName(int.Parse(collectedValues["Scale"]));
                if (loggedinuser != null)
                {
                    vendorService.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(vendorService).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified a vendor service!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(vendorService);
        }

        // GET: VendorServices/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = db.VendorService.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // POST: VendorServices/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorService = db.VendorService.Find(id);
            db.VendorService.Remove(vendorService);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted a vendor service!";
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