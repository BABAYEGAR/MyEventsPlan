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
    public class NotesController : Controller
    {
        private NoteDataContext db = new NoteDataContext();

        // GET: Notes
        public ActionResult Index(long? eventId)
        {
            var notes = db.Notes.Where(n=>n.EventId == eventId).Include(n => n.Event);
            ViewBag.eventId = eventId;
            return View(notes.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoteId,Title,Content,EventId")] Note note)
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
                db.Notes.Add(note);
                db.SaveChanges();
                TempData["note"] = "You have successfully added a built in note!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new {eventId = note.EventId});
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoteId,Title,Content,EventId,CreatedBy,DateCreated")] Note note)
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
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                TempData["note"] = "You have successfully modified the built in note!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index",new {eventId = note.EventId});
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Note note = db.Notes.Find(id);
            db.Notes.Remove(note);
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
