using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorServicesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: VendorServices
        [SessionExpire]
        public ActionResult Index()
        {
            return View(_databaseConnection.VendorServices.ToList());
        }

        // GET: VendorServices/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = _databaseConnection.VendorServices.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // GET: VendorServices/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "VendorServiceId,ServiceName")] VendorService vendorService,
            FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorService.DateCreated = DateTime.Now;
                vendorService.DateLastModified = DateTime.Now;
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
                _databaseConnection.VendorServices.Add(vendorService);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a vendor service!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(vendorService);
        }

        // GET: VendorServices/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = _databaseConnection.VendorServices.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // POST: VendorServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "VendorServiceId,ServiceName,CreatedBy,DateCreated")] VendorService vendorService,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                vendorService.DateLastModified = DateTime.Now;
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
                _databaseConnection.Entry(vendorService).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified a vendor service!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(vendorService);
        }

        // GET: VendorServices/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorService = _databaseConnection.VendorServices.Find(id);
            if (vendorService == null)
                return HttpNotFound();
            return View(vendorService);
        }

        // POST: VendorServices/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorService = _databaseConnection.VendorServices.Find(id);
            _databaseConnection.VendorServices.Remove(vendorService);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted a vendor service!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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