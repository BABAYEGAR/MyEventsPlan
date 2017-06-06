using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;

namespace MyEventPlan.Controllers.VendorPackage
{
    public class VendorPackageSettingsController : Controller
    {
        private readonly VendorPackageSettingDataContext db = new VendorPackageSettingDataContext();

        // GET: VendorPackageSettings
        [SessionExpire]
        public ActionResult Index()
        {
            var vendorPackageSetting = db.VendorPackageSetting.Include(v => v.AppUser).Include(v => v.Vendor)
                .Include(v => v.VendorPackage);
            return View(vendorPackageSetting.ToList());
        }

        // GET: VendorPackageSettings/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageSetting = db.VendorPackageSetting.Find(id);
            if (vendorPackageSetting == null)
                return HttpNotFound();
            return View(vendorPackageSetting);
        }

        // GET: VendorPackageSettings/Create
        [SessionExpire]
        public ActionResult Create()
        {
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname");
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name");
            ViewBag.VendorPackageId = new SelectList(db.VendorPackages, "VendorPackageId", "PackageName");
            return View();
        }

        // POST: VendorPackageSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include =
                "VendorPackageSettingId,Amount,VendorPackageId,StartDate,EndDate,Status,VendorId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorPackageSetting vendorPackageSetting)
        {
            if (ModelState.IsValid)
            {
                db.VendorPackageSetting.Add(vendorPackageSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", vendorPackageSetting.AppUserId);
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name", vendorPackageSetting.VendorId);
            ViewBag.VendorPackageId = new SelectList(db.VendorPackages, "VendorPackageId", "PackageName",
                vendorPackageSetting.VendorPackageId);
            return View(vendorPackageSetting);
        }

        // GET: VendorPackageSettings/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageSetting = db.VendorPackageSetting.Find(id);
            if (vendorPackageSetting == null)
                return HttpNotFound();
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", vendorPackageSetting.AppUserId);
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name", vendorPackageSetting.VendorId);
            ViewBag.VendorPackageId = new SelectList(db.VendorPackages, "VendorPackageId", "PackageName",
                vendorPackageSetting.VendorPackageId);
            return View(vendorPackageSetting);
        }

        // POST: VendorPackageSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "VendorPackageSettingId,Amount,VendorPackageId,StartDate,EndDate,Status,VendorId,AppUserId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorPackageSetting vendorPackageSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorPackageSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppUserId = new SelectList(db.AppUsers, "AppUserId", "Firstname", vendorPackageSetting.AppUserId);
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name", vendorPackageSetting.VendorId);
            ViewBag.VendorPackageId = new SelectList(db.VendorPackages, "VendorPackageId", "PackageName",
                vendorPackageSetting.VendorPackageId);
            return View(vendorPackageSetting);
        }

        // GET: VendorPackageSettings/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorPackageSetting = db.VendorPackageSetting.Find(id);
            if (vendorPackageSetting == null)
                return HttpNotFound();
            return View(vendorPackageSetting);
        }

        // POST: VendorPackageSettings/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorPackageSetting = db.VendorPackageSetting.Find(id);
            db.VendorPackageSetting.Remove(vendorPackageSetting);
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