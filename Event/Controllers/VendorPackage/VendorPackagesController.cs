using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.VendorPackage
{
    public class VendorPackagesController : Controller
    {
        private VendorPackageDataContext db = new VendorPackageDataContext();

        // GET: VendorPackages
        public ActionResult Index()
        {
            return View(db.VendorPackages.ToList());
        }
        // GET: VendorPackages
        public ActionResult Pricing()
        {
            return View(db.VendorPackages.ToList());
        }

        // GET: VendorPackages/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event.Data.Objects.Entities.VendorPackage vendorPackage = db.VendorPackages.Find(id);
            if (vendorPackage == null)
            {
                return HttpNotFound();
            }
            return View(vendorPackage);
        }
        [HttpGet]
        [AllowAnonymous]
        // GET: EventPlanners/Invoice
        public ActionResult Invoice(long id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var selectedPackage = db.VendorPackages.Find(id);
            var subscriptionInvoice = new SubscriptionInvoice();

            //random number
            var generator = new Random();
            var randomNumber = generator.Next(0, 1000000).ToString("D6");

            if (loggedinuser != null)
            {
                subscriptionInvoice.AppUserId = loggedinuser.AppUserId;
                if (loggedinuser.VendorId != null)
                    subscriptionInvoice.VendorId = (long)loggedinuser.VendorId;
                subscriptionInvoice.DateCreated = DateTime.Now;
                subscriptionInvoice.DateLastModified = DateTime.Now;
                subscriptionInvoice.CreatedBy = loggedinuser.AppUserId;
                subscriptionInvoice.LastModifiedBy = loggedinuser.AppUserId;
            }
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: VendorPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorPackageId,Description,PackageName,Amount,PackageGrade")]
        Event.Data.Objects.Entities.VendorPackage vendorPackage,FormCollection collectedValues)
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
                if (db.VendorPackages.Any(n => n.PackageGrade == vendorPackage.PackageGrade))
                {
                    TempData["display"] = "A package already exist with this package grade, Try again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index");
                }
                db.VendorPackages.Add(vendorPackage);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a vendor pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(vendorPackage);
        }

        // GET: VendorPackages/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event.Data.Objects.Entities.VendorPackage vendorPackage = db.VendorPackages.Find(id);
            if (vendorPackage == null)
            {
                return HttpNotFound();
            }
            return View(vendorPackage);
        }

        // POST: VendorPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorPackageId,PackageName,Description,PackageGrade,Amount,CreatedBy,DateCreated")] Event.Data.Objects.Entities.VendorPackage vendorPackage)
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
                db.Entry(vendorPackage).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the vendor pacakge!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { id = vendorPackage.VendorPackageId });
            }
            return View(vendorPackage);
        }

        // GET: VendorPackages/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event.Data.Objects.Entities.VendorPackage vendorPackage = db.VendorPackages.Find(id);
            if (vendorPackage == null)
            {
                return HttpNotFound();
            }
            return View(vendorPackage);
        }

        // POST: VendorPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Event.Data.Objects.Entities.VendorPackage vendorPackage = db.VendorPackages.Find(id);
            db.VendorPackages.Remove(vendorPackage);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the vendor pacakge!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
