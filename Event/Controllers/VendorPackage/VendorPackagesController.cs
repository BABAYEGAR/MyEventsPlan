using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.VendorPackage
{
    public class VendorPackagesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: VendorPackages
        [SessionExpire]
        public ActionResult Index()
        {
            return View(_databaseConnection.VendorPackages.ToList());
        }

        // GET: VendorPackages
        public ActionResult Pricing()
        {
            return View(_databaseConnection.VendorPackages.ToList());
        }

        // GET: VendorPackages/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackage = _databaseConnection.VendorPackages.Find(id);
            if (vendorPackage == null)
                return HttpNotFound();
            return View(vendorPackage);
        }

        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = _databaseConnection.VendorPackages.Find(id);
            var subscriptionInvoice = new SubscriptionInvoice();

            //random number
            var generator = new Random();
            var randomNumber = generator.Next(0, 1000000).ToString("D6");
            subscriptionInvoice.AppUserId = null;
            subscriptionInvoice.DateCreated = DateTime.Now;
            subscriptionInvoice.DateLastModified = DateTime.Now;
            subscriptionInvoice.CreatedBy = null;
            subscriptionInvoice.LastModifiedBy = null;
            subscriptionInvoice.InvoiceNumber = "#" + randomNumber;
            if (selectedPackage != null)
            {
                subscriptionInvoice.PackageId = selectedPackage.VendorPackageId;

                Session["package"] = selectedPackage;
            }
            Session["invoice"] = subscriptionInvoice;
            return View();
        }

        // GET: VendorPackages/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "VendorPackageId,Description,PackageName,Amount,PackageGrade")]
            Event.Data.Objects.Entities.VendorPackage vendorPackage, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorPackage.DateCreated = DateTime.Now;
                vendorPackage.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendorPackage.LastModifiedBy = loggedinuser.AppUserId;
                    vendorPackage.CreatedBy = loggedinuser.AppUserId;
                    vendorPackage.PackageGrade =
                        typeof(VendorPackageEnum).GetEnumName(int.Parse(collectedValues["PackageGrade"]));
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                if (_databaseConnection.VendorPackages.Any(n => n.PackageGrade == vendorPackage.PackageGrade))
                {
                    TempData["display"] = "A package already exist with this package grade, Try again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index");
                }
                _databaseConnection.VendorPackages.Add(vendorPackage);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a vendor pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(vendorPackage);
        }

        // GET: VendorPackages/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackage = _databaseConnection.VendorPackages.Find(id);
            if (vendorPackage == null)
                return HttpNotFound();
            return View(vendorPackage);
        }

        // POST: VendorPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "VendorPackageId,PackageName,Description,PackageGrade,Amount,CreatedBy,DateCreated")]
            Event.Data.Objects.Entities.VendorPackage vendorPackage)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorPackage.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendorPackage.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(vendorPackage).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the vendor pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = vendorPackage.VendorPackageId});
            }
            return View(vendorPackage);
        }

        // GET: VendorPackages/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackage = _databaseConnection.VendorPackages.Find(id);
            if (vendorPackage == null)
                return HttpNotFound();
            return View(vendorPackage);
        }

        // POST: VendorPackages/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorPackage = _databaseConnection.VendorPackages.Find(id);
            _databaseConnection.VendorPackages.Remove(vendorPackage);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the vendor pacakge!";
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