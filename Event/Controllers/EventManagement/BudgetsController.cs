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
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Budgets
        [SessionExpire]
        public ActionResult Index(long id)
        {
            var budgets = _databaseConnection.Budgets.Where(n => n.EventId == id).Include(b => b.Event);
            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var budget = _databaseConnection.Budgets.Find(id);
            if (budget == null)
                return HttpNotFound();
            return View(budget);
        }

        // GET: Budgets/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "BudgetId,ItemName,EstimatedAmount,NegotiatedAmount,ActualAmount,PaidTillDate")]
            Budget budget)
        {
            if (ModelState.IsValid)
            {
                var events = Session["event"] as Event.Data.Objects.Entities.Event;
                if (events != null)
                {
                    var targetBudget = Convert.ToInt64(events.TargetBudget);
                    var eventBudget = _databaseConnection.Budgets.Where(n => n.EventId == events.EventId).ToList();
                    long totalAmount = 0;
                    if (eventBudget.Count > 0)
                    {
                        var sum = _databaseConnection.Budgets.Where(n => n.EventId == events.EventId).Sum(n => n.PaidTillDate);
                        if (sum != null)
                            totalAmount = (long) sum;
                    }

                    if (totalAmount + budget.PaidTillDate < targetBudget)
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
                        _databaseConnection.Budgets.Add(budget);
                        _databaseConnection.SaveChanges();
                        TempData["display"] = "Your have successfully added the budget for an item!";
                        TempData["notificationtype"] = NotificationType.Info.ToString();
                        return RedirectToAction("Index", new {id = events.EventId});
                    }
                }
                TempData["display"] = "Your budget item overides your target budget !";
                TempData["notificationtype"] = NotificationType.Error.ToString();
                if (events != null) return RedirectToAction("Index", new {id = events.EventId});
            }
            return View(budget);
        }

        // GET: Budgets/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var budget = _databaseConnection.Budgets.Find(id);
            if (budget == null)
                return HttpNotFound();
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "BudgetId,ItemName,EstimatedAmount,NegotiatedAmount,ActualAmount,PaidTillDate,EventId,CreatedBy,DateCreated")]
            Budget budget)
        {
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            var eventBudget = _databaseConnection.Budgets.Where(n => n.EventId == events.EventId).ToList();
            long totalAmount = 0;
            if (eventBudget.Count > 0)
            {
                var sum = _databaseConnection.Budgets.Where(n => n.EventId == events.EventId).Sum(n => n.PaidTillDate);
                if (sum != null)
                    totalAmount = (long) sum;
            }
            if (events != null)
            {
                var targetBudget = Convert.ToInt64(events.TargetBudget);
                if (totalAmount + budget.PaidTillDate < targetBudget)
                {
                    var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                    budget.DateLastModified = DateTime.Now;
                    budget.EventId = events.EventId;
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
                    _databaseConnection.Entry(budget).State = EntityState.Modified;
                    _databaseConnection.SaveChanges();
                    TempData["display"] = "Your have successfully modified the budget for the item!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Index");
                }
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var budget = _databaseConnection.Budgets.Find(id);
            if (budget == null)
                return HttpNotFound();
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id, long eventId)
        {
            var budget = _databaseConnection.Budgets.Find(id);
            _databaseConnection.Budgets.Remove(budget);
            _databaseConnection.SaveChanges();
            return RedirectToAction("Index", new {id = eventId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}