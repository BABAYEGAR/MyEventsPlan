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

namespace MyEventPlan.Controllers.EventManagement
{
    public class GuestListsController : Controller
    {
        private GuestListDataContext db = new GuestListDataContext();

        // GET: GuestLists
        public ActionResult Index(long? eventId)
        {
            var guestLists = db.GuestLists.Where(n=>n.EventId == eventId).Include(g => g.Event);
            ViewBag.eventId = eventId;
            return View(guestLists.ToList());
        }

        // GET: GuestLists/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestList guestList = db.GuestLists.Find(id);
            if (guestList == null)
            {
                return HttpNotFound();
            }
            return View(guestList);
        }

        // GET: GuestLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GuestListId,Name,EventId")] GuestList guestList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                guestList.DateCreated = DateTime.Now;
                guestList.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    guestList.LastModifiedBy = loggedinuser.AppUserId;
                    guestList.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.GuestLists.Add(guestList);
                db.SaveChanges();
                return RedirectToAction("Index",new {eventId = guestList.EventId});
            }
            return View(guestList);
        }

        // GET: GuestLists/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestList guestList = db.GuestLists.Find(id);
            if (guestList == null)
            {
                return HttpNotFound();
            }
            return View(guestList);
        }

        // POST: GuestLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GuestListId,Name,EventId,CreatedBy,DateCreated")] GuestList guestList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                guestList.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    guestList.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(guestList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { eventId = guestList.EventId });
            }
            return View(guestList);
        }

        // GET: GuestLists/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuestList guestList = db.GuestLists.Find(id);
            if (guestList == null)
            {
                return HttpNotFound();
            }
            return View(guestList);
        }

        // POST: GuestLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GuestList guestList = db.GuestLists.Find(id);
            db.GuestLists.Remove(guestList);
            db.SaveChanges();
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
