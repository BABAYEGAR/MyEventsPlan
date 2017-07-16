using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorEnquiriesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: VendorEnquiries

        public ActionResult Index()
        {
            var vendorEnquiries = _databaseConnection.VendorEnquiries.Include(v => v.Vendor);
            return View(vendorEnquiries.ToList());
        }

        // GET: VendorEnquiries/Details/5
       
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorEnquiry = _databaseConnection.VendorEnquiries.Find(id);
            if (vendorEnquiry == null)
                return HttpNotFound();
            return View(vendorEnquiry);
        }

        // GET: VendorEnquiries/Create
       
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: VendorEnquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create(
            [Bind(Include =
                "VendorEnquiryId,Name,Email,MobileNumber,EventDate,Note,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorEnquiry vendorEnquiry)
        {
            if (ModelState.IsValid)
            {
                vendorEnquiry.DateCreated = DateTime.Now;
                vendorEnquiry.DateLastModified = DateTime.Now;
                _databaseConnection.VendorEnquiries.Add(vendorEnquiry);
                _databaseConnection.SaveChanges();
                return RedirectToAction("Details", "Vendors", new {id = vendorEnquiry.VendorId});
            }
            return View(vendorEnquiry);
        }

        // GET: VendorEnquiries/Edit/5
       
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorEnquiry = _databaseConnection.VendorEnquiries.Find(id);
            if (vendorEnquiry == null)
                return HttpNotFound();
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorEnquiry.VendorId);
            return View(vendorEnquiry);
        }

        // POST: VendorEnquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit(
            [Bind(Include =
                "VendorEnquiryId,Name,Email,MobileNumber,EventDate,Note,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorEnquiry vendorEnquiry)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(vendorEnquiry).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorEnquiry.VendorId);
            return View(vendorEnquiry);
        }

        // GET: VendorEnquiries/Delete/5
       
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorEnquiry = _databaseConnection.VendorEnquiries.Find(id);
            if (vendorEnquiry == null)
                return HttpNotFound();
            return View(vendorEnquiry);
        }

        // POST: VendorEnquiries/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorEnquiry = _databaseConnection.VendorEnquiries.Find(id);
            _databaseConnection.VendorEnquiries.Remove(vendorEnquiry);
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