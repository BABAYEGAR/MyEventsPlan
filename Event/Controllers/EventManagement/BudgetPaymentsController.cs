using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class BudgetPaymentsController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: BudgetPayments
        public ActionResult Index(long budgetId)
        {
            var budgetPayments = db.BudgetPayments.Include(b => b.Budget).Where(n=>n.BudgetId == budgetId);
            ViewBag.budgetId = budgetId;
            return View(budgetPayments.ToList());
        }

        // GET: BudgetPayments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetPayment budgetPayment = db.BudgetPayments.Find(id);
            if (budgetPayment == null)
            {
                return HttpNotFound();
            }
            return View(budgetPayment);
        }

        // GET: BudgetPayments/Create
        public ActionResult Create()
        {
            //ViewBag.BudgetId = new SelectList(db.Budgets, "BudgetId", "ItemName");
            return View();
        }

        // POST: BudgetPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BudgetPaymentId,AmountPaid,DatePaid,BudgetId")] BudgetPayment budgetPayment)
        {
            if (ModelState.IsValid)
            {
                db.BudgetPayments.Add(budgetPayment);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a payment to the budget!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new{ budgetId = budgetPayment.BudgetId});
            }
            return View(budgetPayment);
        }

        // GET: BudgetPayments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetPayment budgetPayment = db.BudgetPayments.Find(id);
            if (budgetPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "BudgetId", "ItemName", budgetPayment.BudgetId);
            return View(budgetPayment);
        }

        // POST: BudgetPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BudgetPaymentId,AmountPaid,DatePaid,BudgetId")] BudgetPayment budgetPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetPayment).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the budget payment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details", new { id = budgetPayment.BudgetPaymentId });
            }
            //ViewBag.BudgetId = new SelectList(db.Budgets, "BudgetId", "ItemName", budgetPayment.BudgetId);
            return View(budgetPayment);
        }

        // GET: BudgetPayments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetPayment budgetPayment = db.BudgetPayments.Find(id);
            if (budgetPayment == null)
            {
                return HttpNotFound();
            }
            return View(budgetPayment);
        }

        // POST: BudgetPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BudgetPayment budgetPayment = db.BudgetPayments.Find(id);
            db.BudgetPayments.Remove(budgetPayment);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the budget payment!";
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
