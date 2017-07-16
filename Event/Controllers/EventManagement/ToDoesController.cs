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
    public class ToDoesController : Controller
    {
        private EventDataContext db = new EventDataContext();

        // GET: ToDoes
        [SessionExpire]
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var toDos = db.ToDos.Include(t => t.AppUser).
                Include(t => t.Contact).Include(t => t.Event).Include(t => t.EventPlanner).Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId);
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname");
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname");
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View(toDos.ToList());
        }

        // GET: ToDoes/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname");
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname");
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: ToDoes/Create
        [SessionExpire]
        public ActionResult Create()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n=>n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname");
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname");
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name");
            return View();
        }

        // POST: ToDoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create([Bind(Include = "ToDoId,Name,EventId,ContactId,DueDate,Notes,AppUserId,EventPlannerId,SetReminder")] ToDo toDo)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            if (ModelState.IsValid)
            {
                if (loggedinuser != null) toDo.EventPlannerId = loggedinuser.EventPlannerId;
                if (events != null) toDo.EventId = events.EventId;
                db.ToDos.Add(toDo);
                db.SaveChanges();
                TempData["display"] = "You have successfully added a ToDo Item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname", toDo.AppUserId);
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname", toDo.ContactId);
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name", toDo.EventId);
            return View(toDo);
        }

        // GET: ToDoes/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname", toDo.AppUserId);
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname", toDo.ContactId);
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name", toDo.EventId);
            return View(toDo);
        }

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit([Bind(Include = "ToDoId,Name,EventId,ContactId,DueDate,Notes,AppUserId,EventPlannerId,SetReminder")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                TempData["display"] = "You have successfully modified the ToDo Item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.AppUserId = new SelectList(db.AppUsers.Where(n => n.ClientId != null || n.VendorId != null), "AppUserId", "Firstname", toDo.AppUserId);
            ViewBag.ContactId = new SelectList(db.Contacts.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "ContactId", "Firstname", toDo.ContactId);
            ViewBag.EventId = new SelectList(db.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId), "EventId", "Name", toDo.EventId);
            return View(toDo);
        }

        // GET: ToDoes/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            ToDo toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
            db.SaveChanges();
            TempData["display"] = "You have successfully deleted the ToDo Item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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
