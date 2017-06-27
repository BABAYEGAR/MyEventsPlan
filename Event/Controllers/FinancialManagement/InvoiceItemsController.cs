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
    public class InvoiceItemsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: InvoiceItems
        [SessionExpire]
        public ActionResult Index(long? id)
        {
            var invoiceItems = _databaseConnection.InvoiceItems.Where(n => n.InvoiceId == id).Include(i => i.Invoice);
            ViewBag.invoiceId = id;
            return View(invoiceItems.ToList());
        }

        // GET: InvoiceItems/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoiceItem = _databaseConnection.InvoiceItems.Find(id);
            if (invoiceItem == null)
                return HttpNotFound();
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "InvoiceItemId,ItemName,Description,ItemDate,Quantity,UnitCost,InvoiceId")]
            InvoiceItem invoiceItem)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoiceItem.DateCreated = DateTime.Now;
                invoiceItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    invoiceItem.CreatedBy = loggedinuser.AppUserId;
                    invoiceItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.InvoiceItems.Add(invoiceItem);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added an invoice item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = invoiceItem.InvoiceId});
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoiceItem = _databaseConnection.InvoiceItems.Find(id);
            if (invoiceItem == null)
                return HttpNotFound();
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include =
                "InvoiceItemId,ItemName,Description,Quantity,ItemDate,Qantity,UnitCost,InvoiceId,CreatedBy,DateCreated")]
            InvoiceItem invoiceItem)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                invoiceItem.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    invoiceItem.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(invoiceItem).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the  invoice item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {id = invoiceItem.InvoiceId});
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var invoiceItem = _databaseConnection.InvoiceItems.Find(id);
            if (invoiceItem == null)
                return HttpNotFound();
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var invoiceItem = _databaseConnection.InvoiceItems.Find(id);
            var invoiveId = invoiceItem.InvoiceId;
            _databaseConnection.InvoiceItems.Remove(invoiceItem);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the invoice item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {id = invoiveId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}