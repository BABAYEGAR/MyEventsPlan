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
    public class ContactWebsitesController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: ContactWebsites
        [SessionExpire]
        public ActionResult Index(long contactId)
        {
            ViewBag.contactId = contactId;
            var contactWebsite = db.ContactWebsite.Include(c => c.Contact).Where(n=>n.ContactId == contactId);
            return View(contactWebsite.ToList());
        }

        // GET: ContactWebsites/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactWebsite contactWebsite = db.ContactWebsite.Find(id);
            if (contactWebsite == null)
            {
                return HttpNotFound();
            }
            return View(contactWebsite);
        }

        // GET: ContactWebsites/Create
        [SessionExpire]
        public ActionResult Create(long contactId)
        {
            //ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title");
            ViewBag.contactId = contactId;
            return View();
        }

        // POST: ContactWebsites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ContactWebsiteId,Type,Website,ContactId")]
        ContactWebsite contactWebsite,FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                contactWebsite.ContactId = Convert.ToInt64(collection["ContactId"]);
                contactWebsite.Type = typeof(ContactWebsiteType).GetEnumName(int.Parse(collection["Type"]));
                db.ContactWebsite.Add(contactWebsite);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a contact website!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { contactId = contactWebsite.ContactId });
            }
            return View(contactWebsite);
        }

        // GET: ContactWebsites/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactWebsite contactWebsite = db.ContactWebsite.Find(id);
            if (contactWebsite == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title", contactWebsite.ContactId);
            return View(contactWebsite);
        }

        // POST: ContactWebsites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit([Bind(Include = "ContactWebsiteId,Type,Website,ContactId")] ContactWebsite contactWebsite, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                contactWebsite.Type = typeof(ContactWebsiteType).GetEnumName(int.Parse(collection["Type"]));
                db.Entry(contactWebsite).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the website!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new { contactId = contactWebsite.ContactId });
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "ContactId", "Title", contactWebsite.ContactId);
            return View(contactWebsite);
        }

        // GET: ContactWebsites/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactWebsite contactWebsite = db.ContactWebsite.Find(id);
            if (contactWebsite == null)
            {
                return HttpNotFound();
            }
            return View(contactWebsite);
        }

        // POST: ContactWebsites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            ContactWebsite contactWebsite = db.ContactWebsite.Find(id);
            long contactId = (long) contactWebsite.ContactId;
            db.ContactWebsite.Remove(contactWebsite);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the website!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new { contactId = contactId });
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
