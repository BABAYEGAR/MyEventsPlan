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
    public class VendorsController : Controller
    {
        private VendorDataContext db = new VendorDataContext();

        // GET: Vendors
        public ActionResult Index()
        {
            var vendors = db.Vendors.Include(v => v.VendorService);
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View(vendors.ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorId,Name,Address,Email,Mobile,VendorServiceId,EventPlannerId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendor.DateCreated = DateTime.Now;
                vendor.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                    vendor.CreatedBy = loggedinuser.AppUserId;
                    vendor.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                TempData["vendor"] = "You have successfully added a vendor!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", vendor.VendorServiceId);
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", vendor.VendorServiceId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorId,Name,Address,Email,Mobile,VendorServiceId,CreatedBy,DateCreated,EventPlannerId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                vendor.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    vendor.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["vendor"] = "You have successfully modified a vendor!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName");
                return RedirectToAction("Index");
            }
            ViewBag.VendorServiceId = new SelectList(db.VendorService, "VendorServiceId", "ServiceName", vendor.VendorServiceId);
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
            db.SaveChanges();
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
