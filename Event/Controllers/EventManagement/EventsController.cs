using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Calender;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventsController : Controller
    {
        private readonly EventDataContext _db = new EventDataContext();

        // GET: Events
        public ActionResult Index()
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            var events = _db.Event.Include(n => n.EventType).Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
            return View(events.ToList());
        }
        // GET: Events
        public ActionResult Calendar()
        {
            return View();
        }


        //// GET: Events
        public JsonResult GetMyEvents()
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            var events = new CalenderEvent().LoadAllUserEvents(loggedinuser?.EventPlannerId);
            var eventList = from e in events
                            select new
                            {
                                id = e.EventId,
                                title = e.Name + " " + (Convert.ToDateTime(e.EndTime) - Convert.ToDateTime(e.StartTime)) + " mins",
                                start = e.StartDate,
                                end = e.EndDate,
                                color = e.Color,
                                allDay = false
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        // GET: Events/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = _db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            return View(@event);
        }
        public void UpdateEvent(int id, string newEventStart, string newEventEnd)
        {
            new CalenderEvent().UpdateCalendarEvent(id, newEventStart, newEventEnd);
        }

        public bool CreateNewEvent(string title, string newEventStartDate,string newEventEndDate, string newEventStartTime,string newEventEndTime, string color, long budget,
            long plannerId, long type)
        {
            try { new CalenderEvent().CreateNewEvent(title, newEventStartDate, newEventEndDate, newEventStartTime, newEventEndTime, color, budget,
                plannerId, type);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EventId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,EndDate,EndTime")] Event.Data.Objects.Entities.Event @event)
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            var role = Session["role"] as Role;
            if (ModelState.IsValid)
            {
                if (role != null && (loggedinuser != null && role.Name == "Event Planner"))
                {
                    @event.CreatedBy = loggedinuser.AppUserId;
                    @event.DateCreated = DateTime.Now;
                    @event.DateLastModified = DateTime.Now;
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                    @event.Status = EventStausEnum.New.ToString();
                    @event.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Event.Add(@event);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = _db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "EventId,Name,Color,EventTypeId,TargetBudget,StartDate,StartTime,Status,EndDate,EndTime,CreatedBy,DateCreated"
             )] Event.Data.Objects.Entities.Event @event)
        {
            var loggedinuser = Session["planmyleaveloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                @event.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Entry(@event).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var @event = _db.Event.Find(id);
            if (@event == null)
                return HttpNotFound();
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var @event = _db.Event.Find(id);
            _db.Event.Remove(@event);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}