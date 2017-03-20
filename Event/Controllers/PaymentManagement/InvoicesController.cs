using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.PaymentManagement
{
    public class InvoicesController : Controller
    {
        private readonly InvoiceDataContext _db = new InvoiceDataContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            IQueryable<Invoice> invoices = null;
            if ((loggedinuser != null) && (loggedinuser.ClientId != null))
            {
                invoices =
                    _db.Invoices.Where(n => n.ClientId == loggedinuser.ClientId)
                        .Include(i => i.Client)
                        .Include(i => i.EventPlanner);
                return View(invoices.ToList());
            }
            if ((loggedinuser != null) && (loggedinuser.EventPlannerId != null))
            {
                invoices =
                    _db.Invoices.Where(n => n.ClientId == loggedinuser.ClientId)
                        .Include(i => i.Client)
                        .Include(i => i.EventPlanner);
                return View(invoices.ToList());
            }
            return View(invoices.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoice = _db.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.ClientId = new SelectList(_db.Clients.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "ClientId", "Name");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "InvoiceId,InvoiceName,InvoiceNumber,DueDate,ClientId")] Invoice invoice)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoice.DateCreated = DateTime.Now;
                invoice.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    invoice.LastModifiedBy = loggedinuser.AppUserId;
                    invoice.CreatedBy = loggedinuser.AppUserId;
                    if (loggedinuser.EventPlannerId != null)
                        invoice.EventPlannerId = (long) loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Invoices.Add(invoice);
                _db.SaveChanges();
                TempData["display"] = "You have successfully created a new Invoice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(_db.Clients.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "ClientId", "Name");
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoice = _db.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.ClientId = new SelectList(_db.Clients.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "ClientId", "Name");
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "InvoiceId,InvoiceName,InvoiceNumber,DueDate,ClientId,EventPlannerId,CreatedBy,DateCreated")
            ] Invoice invoice)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoice.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    invoice.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Entry(invoice).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["display"] = "You have successfully modified the Invoice!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(_db.Clients.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "ClientId", "Name");
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoice = _db.Invoices.Find(id);
            if (invoice == null)
                return HttpNotFound();
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var invoice = _db.Invoices.Find(id);
            _db.Invoices.Remove(invoice);
            _db.SaveChanges();
            TempData["display"] = "You have successfully deleted the Invoice!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}