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
    public class GuestListsController : Controller
    {
        private readonly GuestListDataContext db = new GuestListDataContext();

        // GET: GuestLists
        [SessionExpire]
        public ActionResult Index(long? eventId)
        {
            var guestLists = db.GuestLists.Where(n => n.EventId == eventId).Include(g => g.Event);
            ViewBag.eventId = eventId;
            return View(guestLists.ToList());
        }

        // GET: GuestLists/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guestList = db.GuestLists.Find(id);
            if (guestList == null)
                return HttpNotFound();
            return View(guestList);
        }

        // GET: GuestLists/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuestLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "GuestListId,Name,EventId")] GuestList guestList)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                guestList.DateCreated = DateTime.Now;
                guestList.DateLastModified = DateTime.Now;
                var listExist = db.GuestLists.Where(m => m.EventId == guestList.EventId && m.Name == guestList.Name)
                    .ToList();
                if (loggedinuser != null)
                {
                    if (listExist.Count > 0)
                    {
                        TempData["display"] = "A guest-list with the same name exist, try another name!";
                        TempData["notificationtype"] = NotificationType.Error.ToString();
                        return RedirectToAction("Index", new {eventId = guestList.EventId});
                    }
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
                TempData["display"] = "You have successfully added a new guest list!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = guestList.EventId});
            }
            return View(guestList);
        }

        // GET: GuestLists/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guestList = db.GuestLists.Find(id);
            if (guestList == null)
                return HttpNotFound();
            return View(guestList);
        }

        // POST: GuestLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
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
                TempData["display"] = "You have successfully modified the guest list!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = guestList.EventId});
            }
            return View(guestList);
        }

        // GET: GuestLists/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guestList = db.GuestLists.Find(id);
            if (guestList == null)
                return HttpNotFound();
            return View(guestList);
        }

        // POST: GuestLists/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var guestList = db.GuestLists.Find(id);
            var eventId = guestList.EventId;
            db.GuestLists.Remove(guestList);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the guest list!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {eventId = guestList.EventId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}