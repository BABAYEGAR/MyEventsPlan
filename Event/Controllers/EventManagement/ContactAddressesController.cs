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
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class ContactAddressesController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: ContactAddresses
        [SessionExpire]
        public ActionResult Index(long contactId)
        {
            ViewBag.contactId = contactId;
            var contactAddress = db.ContactAddress.Include(c => c.Contact).Where(n=>n.ContactId == contactId);
            return View(contactAddress.ToList());
        }

        // GET: ContactAddresses/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAddress contactAddress = db.ContactAddress.Find(id);
            if (contactAddress == null)
            {
                return HttpNotFound();
            }
            return View(contactAddress);
        }

        // GET: ContactAddresses/Create
        [SessionExpire]
        public ActionResult Create()
        {
            //ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title");
            return View();
        }

        // POST: ContactAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ContactAddressId,Type,Street1,Street2,PostalCode," +
                                                   "State,Country,ContactId")] ContactAddress contactAddress,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                contactAddress.Type = typeof(ContactAddressType).GetEnumName(int.Parse(collection["Type"]));
                contactAddress.ContactId = Convert.ToInt64(collection["ContactId"]);
                db.ContactAddress.Add(contactAddress);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a website!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new{ contactId  = contactAddress.ContactId});
            }

            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title", contactAddress.ContactId);
            return View(contactAddress);
        }

        // GET: ContactAddresses/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAddress contactAddress = db.ContactAddress.Find(id);
            if (contactAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title", contactAddress.ContactId);
            return View(contactAddress);
        }

        // POST: ContactAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit([Bind(Include = "ContactAddressId,Type,Street1," +
                                                 "Street2,PostalCode,State,Country,ContactId")] ContactAddress contactAddress, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                contactAddress.Type = typeof(ContactAddressType).GetEnumName(int.Parse(collection["Type"]));
                db.Entry(contactAddress).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the website!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { contactId = contactAddress.ContactId });
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title", contactAddress.ContactId);
            return View(contactAddress);
        }

        // GET: ContactAddresses/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactAddress contactAddress = db.ContactAddress.Find(id);
            if (contactAddress == null)
            {
                return HttpNotFound();
            }
            return View(contactAddress);
        }

        // POST: ContactAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            ContactAddress contactAddress = db.ContactAddress.Find(id);
            long addressId = id;
            db.ContactAddress.Remove(contactAddress);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the website!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new { contactId = addressId });
        }
        [SessionExpire]
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
