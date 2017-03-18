using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.EmailService;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class GuestsController : Controller
    {
        private readonly GuestDataContext db = new GuestDataContext();

        // GET: Guests
        public ActionResult Index(long? guestListId)
        {
            var guests =
                db.Guests.Where(n => n.GuestListId == guestListId).Include(g => g.Event).Include(g => g.GuestList);
            ViewBag.guestListId = guestListId;
            return View(guests.ToList());
        }

        // GET: Guests/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guest = db.Guests.Find(id);
            if (guest == null)
                return HttpNotFound();
            return View(guest);
        }

        // GET: Guests/GuestAttending/5
        public ActionResult GuestAttending(long? id)
        {
            var guest = db.Guests.Find(id);
            guest.Status = GuestStatusEnum.Attending.ToString();
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new {guestListId = guest.GuestListId});
        }

        // GET: Guests/GuestNotAttending/5
        public ActionResult GuestNotAttending(long? id)
        {
            var guest = db.Guests.Find(id);
            guest.Status = GuestStatusEnum.NotAttending.ToString();
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new {guestListId = guest.GuestListId});
        }
        // GET: Guests/GuestAttending/5
        public ActionResult LoggedInGuestAttending(long? id)
        {
            var guest = db.Guests.Find(id);
            guest.Status = GuestStatusEnum.Attending.ToString();
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { guestListId = guest.GuestListId });
        }

        // GET: Guests/GuestNotAttending/5
        public ActionResult LoggedInGuestNotAttending(long? id)
        {
            var guest = db.Guests.Find(id);
            guest.Status = GuestStatusEnum.NotAttending.ToString();
            db.Entry(guest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { guestListId = guest.GuestListId });
        }

        // GET: Guests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GuestId,Name,Email,PhoneNumber,EventId,GuestListId")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                guest.DateCreated = DateTime.Now;
                guest.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    guest.LastModifiedBy = loggedinuser.AppUserId;
                    guest.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Guests.Add(guest);
                db.SaveChanges();
               
                var eventName = db.Event.Find(guest.EventId);
                new MailerDaemon().NewGuest(guest,eventName.Name);
                TempData["display"] = "You have successfully added the guest!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {guestListId = guest.GuestListId});
            }
            return View(guest);
        }

        // GET: Guests/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guest = db.Guests.Find(id);
            if (guest == null)
                return HttpNotFound();
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "GuestId,Name,Email,PhoneNumber,EventId,GuestListId,EventPlannerId,CreatedBy,DateCreated")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                guest.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    guest.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the guest!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        // GET: Guests/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var guest = db.Guests.Find(id);
            if (guest == null)
                return HttpNotFound();
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
            db.SaveChanges();
            TempData["guest"] = "You have successfully deleted the guest!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}