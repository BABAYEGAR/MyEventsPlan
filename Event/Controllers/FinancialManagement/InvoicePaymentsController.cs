using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.FinancialManagement
{
    public class InvoicePaymentsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: InvoicePayments
        [SessionExpire]
        public ActionResult Index(long? id)
        {
            var invoicePayments = _databaseConnection.InvoicePayments.Where(n => n.InvoiceId == id).Include(i => i.Invoice);
            ViewBag.invoiceId = id;
            return View(invoicePayments.ToList());
        }

        // GET: InvoicePayments/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoicePayment = _databaseConnection.InvoicePayments.Find(id);
            if (invoicePayment == null)
                return HttpNotFound();
            return View(invoicePayment);
        }

        // GET: InvoicePayments/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoicePayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "InvoicePaymentId,Amount,Reference,PaymentDate,InvoiceId")] InvoicePayment invoicePayment,FormCollection collection)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoicePayment.DateCreated = DateTime.Now;
                invoicePayment.DateLastModified = DateTime.Now;
                invoicePayment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                if (loggedinuser != null)
                {
                    invoicePayment.CreatedBy = loggedinuser.AppUserId;
                    invoicePayment.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.InvoicePayments.Add(invoicePayment);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a payment to the invoice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = invoicePayment.InvoiceId});
            }
            return View(invoicePayment);
        }

        // GET: InvoicePayments/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoicePayment = _databaseConnection.InvoicePayments.Find(id);
            if (invoicePayment == null)
                return HttpNotFound();
            return View(invoicePayment);
        }

        // POST: InvoicePayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "InvoicePaymentId,Amount,Reference,PaymentDate,InvoiceId,CreatedBy,DateCreated")]
            InvoicePayment invoicePayment)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoicePayment.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    invoicePayment.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(invoicePayment).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the payment for the invoice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = invoicePayment.InvoiceId});
            }
            return View(invoicePayment);
        }

        // GET: InvoicePayments/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoicePayment = _databaseConnection.InvoicePayments.Find(id);
            if (invoicePayment == null)
                return HttpNotFound();
            return View(invoicePayment);
        }

        // POST: InvoicePayments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var invoicePayment = _databaseConnection.InvoicePayments.Find(id);
            var invoiceId = invoicePayment.InvoiceId;
            _databaseConnection.InvoicePayments.Remove(invoicePayment);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the payment for the invoice!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {id = invoiceId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}