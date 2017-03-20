using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class BudgetsController : Controller
    {
        private BudgetDataContext db = new BudgetDataContext();

        // GET: Budgets
        public ActionResult Index(long id)
        {
            var budgets = db.Budgets.Where(n=>n.EventId == id).Include(b => b.Event);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BudgetId,ItemName,EstimatedAmount,NegotiatedAmount,ActualAmount,PaidTillDate")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                var events = Session["event"] as Event.Data.Objects.Entities.Event;
                var eventBudget = db.Budgets.Where(n => n.EventId == events.EventId).ToList();
                long totalAmount = 0;
                if (eventBudget.Count > 0)
                {
                    totalAmount = db.Budgets.Where(n => n.EventId == events.EventId).Sum(n => n.PaidTillDate);
                }
           
                if (events != null &&  totalAmount + budget.PaidTillDate < events.TargetBudget )
                {
                    var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                    budget.DateCreated = DateTime.Now;
                    budget.DateLastModified = DateTime.Now;
                    budget.EventId = events.EventId;
                    if (loggedinuser != null)
                    {
                        budget.LastModifiedBy = loggedinuser.AppUserId;
                        budget.CreatedBy = loggedinuser.AppUserId;
                        budget.AmountStillDue = budget.ActualAmount - budget.PaidTillDate;
                    }
                    else
                    {
                        TempData["login"] = "Your session has expired, Login again!";
                        TempData["notificationtype"] = NotificationType.Info.ToString();
                        return RedirectToAction("Login", "Account");
                    }
                    db.Budgets.Add(budget);
                    db.SaveChanges();
                    TempData["display"] = "Your have successfully added the budget for an item!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index", new {id = events.EventId});
                }
                TempData["display"] = "Your budget item overides your target budget !";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                if (events != null) return RedirectToAction("Index", new { id = events.EventId });
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BudgetId,ItemName,EstimatedAmount,NegotiatedAmount,ActualAmount,PaidTillDate,EventId,CreatedBy,DateCreated")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                budget.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    budget.LastModifiedBy = loggedinuser.AppUserId;
                    budget.AmountStillDue = budget.ActualAmount - budget.PaidTillDate;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "Your have successfully modified the budget for the item!";
                TempData["notificationtype"] = NotificationType.Info.ToString();
                return RedirectToAction("Index");
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id,long eventId)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index",new {id = eventId});
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
