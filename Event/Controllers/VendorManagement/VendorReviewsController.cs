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

namespace MyEventPlan.Controllers.VendorManagement
{
    public class VendorReviewsController : Controller
    {
        private VendorReviewDataContext db = new VendorReviewDataContext();
        private VendorDataContext dbc = new VendorDataContext();

        // GET: VendorReviews
        public ActionResult Index()
        {
            var vendorReviews = db.VendorReviews.Include(v => v.Vendor);
            return View(vendorReviews.ToList());
        }

        // GET: VendorReviews/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorReview vendorReview = db.VendorReviews.Find(id);
            if (vendorReview == null)
            {
                return HttpNotFound();
            }
            return View(vendorReview);
        }

        // GET: VendorReviews/Create
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name");
            return View();
        }

        // POST: VendorReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorReviewId,ReviewerName,ReviewerEmail,ReviewTitle,ReviewBody,Rating,VendorId,CreatedBy" +
                                                   ",DateCreated,DateLastModified,LastModifiedBy")] VendorReview vendorReview,FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var rating = collectedValues["Rating"];
                vendorReview.Rating = Convert.ToInt64(rating);
                vendorReview.DateCreated = DateTime.Now;
                vendorReview.DateLastModified = DateTime.Now;

                db.VendorReviews.Add(vendorReview);
                db.SaveChanges();

                var vendor = dbc.Vendors.Find(vendorReview.VendorId);
                var reviews = db.VendorReviews.Where(n => n.VendorId == vendorReview.VendorId).ToList();
                long? totalRatings = 0;
                long? totalPossibleRatings = 0;
                double? ratingValue = 0;
                long ratings = 0;
                if (reviews.Count > 0)
                {
                    totalRatings = reviews.Sum(n => n.Rating);
                    totalPossibleRatings = reviews.Count * 5;
                    ratingValue = totalRatings * 5 / totalPossibleRatings;
                    if (ratingValue != null)
                    {
                        ratings = (long)Math.Round((double)ratingValue);
                        vendor.AverageRating = ratings;
                    }
                }
                dbc.Entry(vendor).State = EntityState.Modified;
                dbc.SaveChanges();
                return RedirectToAction("Details", "Vendors", new { id = vendorReview.VendorId });
            }
            return View(vendorReview);
        }

        // GET: VendorReviews/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorReview vendorReview = db.VendorReviews.Find(id);
            if (vendorReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name", vendorReview.VendorId);
            return View(vendorReview);
        }

        // POST: VendorReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorReviewId,ReviewerName,ReviewerEmail,ReviewTitle,ReviewBody,Rating,VendorId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] VendorReview vendorReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "VendorId", "Name", vendorReview.VendorId);
            return View(vendorReview);
        }

        // GET: VendorReviews/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorReview vendorReview = db.VendorReviews.Find(id);
            if (vendorReview == null)
            {
                return HttpNotFound();
            }
            return View(vendorReview);
        }

        // POST: VendorReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            VendorReview vendorReview = db.VendorReviews.Find(id);
            db.VendorReviews.Remove(vendorReview);
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
