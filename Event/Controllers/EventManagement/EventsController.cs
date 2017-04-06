using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Calender;
using MyEventPlan.Data.Service.Enum;
using ColorConverter = System.Drawing.ColorConverter;

namespace MyEventPlan.Controllers.EventManagement
{
    public class EventsController : Controller
    {
        private readonly EventDataContext _db = new EventDataContext();

        // GET: Events
        public ActionResult Index()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            IQueryable<Event.Data.Objects.Entities.Event> events = null;
            if ((loggedinuser != null) && (loggedinuser.EventPlannerId != null))
            {
                events =
                    _db.Event.OrderByDescending(n => n.StartDate)
                        .Include(n => n.EventType)
                        .Where(n => n.EventPlannerId == loggedinuser.EventPlannerId);
                return View(events.ToList());
            }
            if ((loggedinuser != null) && (loggedinuser.ClientId != null))
            {
                var client = _db.Clients.Find(loggedinuser.ClientId);
                events =
                    _db.Event.OrderByDescending(n => n.StartDate)
                        .Include(n => n.EventType)
                        .Where(n => n.EventId == client.EventId);
                return View(events.ToList());
            }
            if ((loggedinuser != null) && (loggedinuser.VendorId != null))
            {
                events =
                    from a in _db.Event
                    join b in _db.EventVendorMappings on a.EventId equals b.EventId
                    where b.VendorId == loggedinuser.VendorId
                    select a;

                return View(events.ToList());
            }
            List<Event.Data.Objects.Entities.Event> list = new List<Event.Data.Objects.Entities.Event>();
            foreach (var @event in events)
                list.Add(@event);
            return View(list);
        }

        // GET: Events
        public ActionResult Calendar()
        {
            return View();
        }

        //// GET: Events
        public JsonResult GetMyEvents()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var events = new CalenderEvent().LoadAllUserEvents(loggedinuser?.EventPlannerId);
            var eventList = from e in events
                select new
                {
                    id = e.EventId,
                    title = e.Name +" ... Starts from " + e.StartTime + "To " + e.EndTime,
                    start = e.StartDate,
                    end = e.EndDate,
                    color = e.Color,
                    allDay = false,
                    backgroundColor = e.Color,
                    startTime = e.StartTime,
                    endTime = e.EndTime
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
            Session["event"] = @event;
            return View(@event);
        }

        public void UpdateEvent(int id, string newEventStart, string newEventEnd)
        {
            new CalenderEvent().UpdateCalendarEvent(id, newEventStart, newEventEnd);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCalendarEvent(FormCollection collectedValues)
        {
            var eventId = Convert.ToInt64(collectedValues["EventId"]);
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var calendarEvent = _db.Event.Find(eventId);
            if (calendarEvent != null)
            {
                calendarEvent.DateLastModified = DateTime.Now;
                if (loggedinuser != null) calendarEvent.LastModifiedBy = loggedinuser.AppUserId;
                calendarEvent.Color = collectedValues["Color"];
                calendarEvent.StartDate = Convert.ToDateTime(collectedValues["StartDate"]);
                calendarEvent.EndDate = Convert.ToDateTime(collectedValues["EndDate"]);
                calendarEvent.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                calendarEvent.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                _db.Entry(calendarEvent).State = EntityState.Modified;
            }
            _db.SaveChanges();
            return RedirectToAction("Calendar");
        }

        public bool CreateNewEvent(string title, string newEventStartDate, string newEventEndDate,
            long appUserId, string color, long budget,
            long plannerId, long type, string eventDate)
        {
            try
            {
                new CalenderEvent().CreateNewEvent(title, newEventStartDate, newEventEndDate, appUserId, color, budget,
                    plannerId, type, eventDate);
            }
            catch (Exception)
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
            [Bind(Include = "EventId,Name,Color,EventTypeId,TargetBudget,EventDate,StartDate,EndDate")] Event.Data.Objects.Entities.Event @event, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = Session["role"] as Role;
            
            if (ModelState.IsValid)
            {
                if ((role != null) && (loggedinuser != null) && (role.Name == "Event Planner"))
                {
                    @event.CreatedBy = loggedinuser.AppUserId;
                    @event.DateCreated = DateTime.Now;
                    @event.DateLastModified = DateTime.Now;
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                    @event.TargetBudget = @event.TargetBudget.Replace(",", "");
                    @event.Status = EventStausEnum.New.ToString();
                    @event.EventPlannerId = loggedinuser.EventPlannerId;
                    @event.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    @event.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                   

                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Event.Add(@event);
                _db.SaveChanges();
                if (@event.EventId > 0)
                {
                    //package data
                    var packageData =
                        _db.EventPlannerPackages.FirstOrDefault(n => n.Status == PackageStatusEnum.Active.ToString());
                    if (packageData != null && packageData.SubscribedEvent < packageData.AllowedEvent)
                    {
                        packageData.SubscribedEvent = packageData.SubscribedEvent + 1;
                    }
                    if (packageData != null && packageData.SubscribedEvent >= packageData.AllowedEvent)
                    {
                        packageData.Status = PackageStatusEnum.Inactive.ToString();
                    }
                    if (packageData != null)
                    {
                        packageData.LastModifiedBy = loggedinuser.AppUserId;
                        packageData.DateLastModified = DateTime.Now;
                    }
                    _db.Entry(packageData).State = EntityState.Modified;
                    _db.SaveChanges();

                }
                //display notification
                TempData["display"] = "You have successfully added an event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
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
                     "EventId,Name,Color,EventTypeId,TargetBudget,EventDate,EventPlannerId,StartDate,Status,EndDate,CreatedBy,DateCreated"
             )] Event.Data.Objects.Entities.Event @event, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                @event.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    @event.LastModifiedBy = loggedinuser.AppUserId;
                    @event.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    @event.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                    @event.TargetBudget = @event.TargetBudget.Replace(",", "");
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Entry(@event).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["display"] = "You have successfully modified an event!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
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
            TempData["display"] = "You have successfully deleted an event!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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