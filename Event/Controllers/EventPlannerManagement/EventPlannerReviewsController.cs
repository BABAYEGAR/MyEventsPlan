using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;

namespace MyEventPlan.Controllers.EventPlannerManagement
{
    public class EventPlannerReviewsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: EventPlannerReviews
     
        public ActionResult Index()
        {
            var eventPlannerReviews = _databaseConnection.EventPlannerReviews.Include(e => e.EventPlanner);
            return View(eventPlannerReviews.ToList());
        }

        // GET: EventPlannerReviews/Details/5
     
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerReview = _databaseConnection.EventPlannerReviews.Find(id);
            if (eventPlannerReview == null)
                return HttpNotFound();
            return View(eventPlannerReview);
        }

        // GET: EventPlannerReviews/Create
     
        public ActionResult Create()
        {
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Name");
            return View();
        }

        // POST: EventPlannerReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public ActionResult Create([Bind(Include = "EventPlannerReviewId,ReviewerName,ReviewerEmail,ReviewTitle," +
                                                   "ReviewBody,Rating,EventPlannerId,CreatedBy,DateCreated" +
                                                   ",DateLastModified,LastModifiedBy")]
            EventPlannerReview eventPlannerReview, FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var rating = collectedValues["Rating"];
                eventPlannerReview.Rating = Convert.ToInt64(rating);
                eventPlannerReview.DateCreated = DateTime.Now;
                eventPlannerReview.DateLastModified = DateTime.Now;

                _databaseConnection.EventPlannerReviews.Add(eventPlannerReview);
                _databaseConnection.SaveChanges();

                var planner = _databaseConnection.EventPlanners.Find(eventPlannerReview.EventPlannerId);
                var reviews = _databaseConnection.EventPlannerReviews.Where(n => n.EventPlannerId == eventPlannerReview.EventPlannerId)
                    .ToList();
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
                        ratings = (long) Math.Round((double) ratingValue);
                        planner.AverageRating = ratings;
                    }
                }
                _databaseConnection.Entry(planner).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("EventPlannerDetails", "EventPlanners",
                    new {id = eventPlannerReview.EventPlannerId});
            }
            return RedirectToAction("EventPlannerDetails", "EventPlanners",
                new {id = eventPlannerReview.EventPlannerId});
        }

        // GET: EventPlannerReviews/Edit/5
     
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerReview = _databaseConnection.EventPlannerReviews.Find(id);
            if (eventPlannerReview == null)
                return HttpNotFound();
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Name",
                eventPlannerReview.EventPlannerId);
            return View(eventPlannerReview);
        }

        // POST: EventPlannerReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public ActionResult Edit(
            [Bind(Include =
                "EventPlannerReviewId,ReviewerName,ReviewerEmail,ReviewTitle,ReviewBody,Rating,EventPlannerId,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")]
            EventPlannerReview eventPlannerReview)
        {
            if (ModelState.IsValid)
            {
                _databaseConnection.Entry(eventPlannerReview).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventPlannerId = new SelectList(_databaseConnection.EventPlanners, "EventPlannerId", "Name",
                eventPlannerReview.EventPlannerId);
            return View(eventPlannerReview);
        }

        // GET: EventPlannerReviews/Delete/5
     
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var eventPlannerReview = _databaseConnection.EventPlannerReviews.Find(id);
            if (eventPlannerReview == null)
                return HttpNotFound();
            return View(eventPlannerReview);
        }

        // POST: EventPlannerReviews/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
     
        public ActionResult DeleteConfirmed(long id)
        {
            var eventPlannerReview = _databaseConnection.EventPlannerReviews.Find(id);
            _databaseConnection.EventPlannerReviews.Remove(eventPlannerReview);
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