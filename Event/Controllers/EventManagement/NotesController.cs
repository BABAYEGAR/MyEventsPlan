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
    public class NotesController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Notes
        [SessionExpire]
        public ActionResult Index(long? eventId)
        {
            var notes = _databaseConnection.Notes.Where(n => n.EventId == eventId).Include(n => n.Event);
            ViewBag.eventId = eventId;
            return View(notes.ToList());
        }

        // GET: Notes/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var note = _databaseConnection.Notes.Find(id);
            if (note == null)
                return HttpNotFound();
            return View(note);
        }

        // GET: Notes/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "NoteId,Title,Content,ShowToClient,EventId")] Note note)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                note.DateCreated = DateTime.Now;
                note.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    note.LastModifiedBy = loggedinuser.AppUserId;
                    note.CreatedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Notes.Add(note);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added a built in note!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = note.EventId});
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var note = _databaseConnection.Notes.Find(id);
            if (note == null)
                return HttpNotFound();
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(Include = "NoteId,Title,Content,ShowToClient,EventId,CreatedBy,DateCreated")] Note note)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                note.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    note.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(note).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified the built in note!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = note.EventId});
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var note = _databaseConnection.Notes.Find(id);
            if (note == null)
                return HttpNotFound();
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var note = _databaseConnection.Notes.Find(id);
            var eventId = note.EventId;
            _databaseConnection.Notes.Remove(note);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully deleted the built in note!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", new {eventId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _databaseConnection.Dispose();
            base.Dispose(disposing);
        }
    }
}