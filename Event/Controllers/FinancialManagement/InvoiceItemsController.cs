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
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.FinancialManagement
{
    public class InvoiceItemsController : Controller
    {
        private InvoiceItemDataContext db = new InvoiceItemDataContext();

        // GET: InvoiceItems
        public ActionResult Index(long? id)
        {
            var invoiceItems = db.InvoiceItems.Where(n=>n.InvoiceId == id).Include(i => i.Invoice);
            return View(invoiceItems.ToList());
        }

        // GET: InvoiceItems/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceItemId,ItemName,Description,ItemDate,Qantity,UnitCost,InvoiceId")] InvoiceItem invoiceItem)
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
                db.InvoiceItems.Add(invoiceItem);
                db.SaveChanges();
                TempData["display"] = "You have successfully added an invoice item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new {id = invoiceItem.InvoiceId});
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceItemId,ItemName,Description,ItemDate,Qantity,UnitCost,InvoiceId,CreatedBy,DateCreated")] InvoiceItem invoiceItem)
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
                db.Entry(invoiceItem).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the  invoice item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new {id = invoiceItem.InvoiceId});
            }
            return View(invoiceItem);
        }

        // GET: InvoiceItems/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            if (invoiceItem == null)
            {
                return HttpNotFound();
            }
            return View(invoiceItem);
        }

        // POST: InvoiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            InvoiceItem invoiceItem = db.InvoiceItems.Find(id);
            long? invoiveId = invoiceItem.InvoiceId;
            db.InvoiceItems.Remove(invoiceItem);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the invoice item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index",new {id = invoiveId});
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
