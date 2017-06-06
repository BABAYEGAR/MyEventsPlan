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
    public class VendorPackageItemsController : Controller
    {
        private readonly VendorPackageItemDataContext db = new VendorPackageItemDataContext();

        // GET: VendorPackageItems
        [SessionExpire]
        public ActionResult Index(long? id)
        {
            var vendorPackageItems = db.VendorPackageItems.Include(v => v.VendorPackage)
                .Where(n => n.VendorPackageId == id);
            ViewBag.packageId = id;
            return View(vendorPackageItems.ToList());
        }

        // GET: VendorPackageItems/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageItem = db.VendorPackageItems.Find(id);
            if (vendorPackageItem == null)
                return HttpNotFound();
            return View(vendorPackageItem);
        }

        // GET: VendorPackageItems/Create
        [SessionExpire]
        public ActionResult Create(long? packageId)
        {
            ViewBag.packageId = packageId;
            return View();
        }

        // POST: VendorPackageItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "VendorPackageItemId,ItemName,Amount,VendorPackageId")] VendorPackageItem vendorPackageItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorPackageItem.DateCreated = DateTime.Now;
                vendorPackageItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendorPackageItem.LastModifiedBy = loggedinuser.AppUserId;
                    vendorPackageItem.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.VendorPackageItems.Add(vendorPackageItem);
                db.SaveChanges();

                var package = db.VendorPackages.Find(vendorPackageItem.VendorPackageId);
                if (package != null) package.Amount = db.VendorPackageItems.Sum(m => m.Amount);

                db.Entry(vendorPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["display"] = "You have successfully added an item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = vendorPackageItem.VendorPackageId});
            }
            return View(vendorPackageItem);
        }

        // GET: VendorPackageItems/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageItem = db.VendorPackageItems.Find(id);
            if (vendorPackageItem == null)
                return HttpNotFound();
            return View(vendorPackageItem);
        }

        // POST: VendorPackageItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "VendorPackageItemId,ItemName,Amount,VendorPackageId,CreatedBy,DateCreated")]
            VendorPackageItem vendorPackageItem)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendorPackageItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendorPackageItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(vendorPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                var package = db.VendorPackages.Find(vendorPackageItem.VendorPackageId);
                if (package != null) package.Amount = db.VendorPackageItems.Sum(m => m.Amount);

                db.Entry(vendorPackageItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["display"] = "You have successfully modified the item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(vendorPackageItem);
        }

        // GET: VendorPackageItems/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageItem = db.VendorPackageItems.Find(id);
            if (vendorPackageItem == null)
                return HttpNotFound();
            return View(vendorPackageItem);
        }

        // POST: VendorPackageItems/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorPackageItem = db.VendorPackageItems.Find(id);
            db.VendorPackageItems.Remove(vendorPackageItem);
            db.SaveChanges();
            var package = db.VendorPackages.Find(vendorPackageItem.VendorPackageId);
            if (package != null) package.Amount = db.VendorPackageItems.Sum(m => m.Amount) - vendorPackageItem.Amount;

            db.Entry(vendorPackageItem).State = EntityState.Modified;
            db.SaveChanges();

            TempData["display"] = "You have successfully deleted the item!";
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