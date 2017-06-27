using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorReviewsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: VendorReviews

        public ActionResult Index()
        {
            var vendorReviews = _databaseConnection.VendorReviews.Include(v => v.Vendor);
            return View(vendorReviews.ToList());
        }

        // GET: VendorReviews/Details/5
       
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorReview = _databaseConnection.VendorReviews.Find(id);
            if (vendorReview == null)
                return HttpNotFound();
            return View(vendorReview);
        }

        // GET: VendorReviews/Create
       
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: VendorReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create([Bind(Include =
            "VendorReviewId,ReviewerName,ReviewerEmail,ReviewTitle,ReviewBody,Rating,VendorId,CreatedBy" +
            ",DateCreated,DateLastModified,LastModifiedBy")] VendorReview vendorReview, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var rating = collectedValues["Rating"];
                vendorReview.Rating = Convert.ToInt64(rating);
                vendorReview.DateCreated = DateTime.Now;
                vendorReview.DateLastModified = DateTime.Now;

                _databaseConnection.VendorReviews.Add(vendorReview);
                _databaseConnection.SaveChanges();

                var vendor = _databaseConnection.Vendors.Find(vendorReview.VendorId);
                var reviews = _databaseConnection.VendorReviews.Where(n => n.VendorId == vendorReview.VendorId).ToList();
                if (reviews.Count > 0)
                {
                    var totalRatings = reviews.Sum(n => n.Rating);
                    long? totalPossibleRatings = reviews.Count * 5;
                    double? ratingValue = totalRatings * 5 / totalPossibleRatings;
                    if (ratingValue != null)
                    {
                        var ratings = (long) Math.Round((double) ratingValue);
                        if (vendor != null) vendor.AverageRating = ratings;
                    }
                }
                _databaseConnection.Entry(vendor).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Details", "Vendors", new {id = vendorReview.VendorId});
            }
            return View(vendorReview);
        }

        // GET: VendorReviews/Edit/5
       
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorReview = _databaseConnection.VendorReviews.Find(id);
            if (vendorReview == null)
                return HttpNotFound();
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorReview.VendorId);
            return View(vendorReview);
        }

        // POST: VendorReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit(
            [Bind(Include =
                "VendorReviewId,ReviewerName,ReviewerEmail,ReviewTitle,ReviewBody,Rating,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            VendorReview vendorReview)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(vendorReview).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(_databaseConnection.Vendors, "VendorId", "Name", vendorReview.VendorId);
            return View(vendorReview);
        }

        // GET: VendorReviews/Delete/5
       
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var vendorReview = _databaseConnection.VendorReviews.Find(id);
            if (vendorReview == null)
                return HttpNotFound();
            return View(vendorReview);
        }

        // POST: VendorReviews/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
       
        public ActionResult DeleteConfirmed(long id)
        {
            var vendorReview = _databaseConnection.VendorReviews.Find(id);
            _databaseConnection.VendorReviews.Remove(vendorReview);
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