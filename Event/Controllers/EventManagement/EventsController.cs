using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name");
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
            var list = new List<Event.Data.Objects.Entities.Event>();
            foreach (var @event in events)
                list.Add(@event);
            return View(list);
        }
        // GET: Events/QuickLunch
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuickLunch(FormCollection collectedValues)
        {
            long eventId = Convert.ToInt64(collectedValues["quickLaunchJob"]);
            string tool = collectedValues["quickLaunchTool"];
            if (tool == "dashboard")
            {
                return RedirectToAction("Index", "Events", new { id = eventId });
            }
            if (tool == "appointments")
            {
                return RedirectToAction("Index","Appointments", new { eventId = eventId });
            }
            if (tool == "notes")
            {
                return RedirectToAction("Index", "Notes", new { eventId = eventId });
            }
            if (tool == "checklists")
            {
                return RedirectToAction("Index", "CheckLists", new { eventId = eventId });
            }
            if (tool == "budgets")
            {
                return RedirectToAction("Index", "Budgets", new { id = eventId });
            }
            if (tool == "calendar")
            {
                return RedirectToAction("Calendar", "Events");
            }
            if (tool == "invoices")
            {
                return RedirectToAction("Index", "Invoices", new { id = eventId });
            }
            if (tool == "attendees")
            {
                return RedirectToAction("Index", "GuestLists", new { eventId = eventId });
            }
            return RedirectToAction("Dashboard", "Home");
        }

        // GET: Events
        public ActionResult Calendar()
        {
            ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name");
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
                    title = e.Name,
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
            if (@event != null)
            {
                ViewBag.EventTypeId = new SelectList(_db.EventTypes, "EventTypeId", "Name", @event.EventTypeId);
                //recent event details
                var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
                ViewBag.events = @event;
                ViewBag.guestList = _db.GuestLists.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.checkList = _db.CheckLists.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.clients = _db.Clients.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.resources = _db.EventResourceMapping.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.appointments = _db.Appointments.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.invoice = _db.Invoices.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.staff = _db.StaffEventMapping.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.notes = _db.Notes.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.vendors = _db.EventVendorMappings.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.budget = _db.Budgets.Where(n => n.EventId == @event.EventId).ToList();
                ViewBag.task = _db.Tasks.Where(n => n.EventId == @event.EventId).ToList();

                ViewBag.remainingDays = @event.EventDate.Subtract(DateTime.Now).Days;
                Session["event"] = @event;
                return View(@event);
            }
            return View();
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
                calendarEvent.Name = collectedValues["Name"];
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
                    if ((packageData != null) && (packageData.SubscribedEvent < packageData.AllowedEvent))
                        packageData.SubscribedEvent = packageData.SubscribedEvent + 1;
                    if ((packageData != null) && (packageData.SubscribedEvent >= packageData.AllowedEvent))
                        packageData.Status = PackageStatusEnum.Inactive.ToString();
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
                return RedirectToAction("Details", new {id = @event.EventId});
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
            //get all objects that has the eventId as a property
            var clients = _db.Clients.Where(n => n.EventId == @event.EventId);
            var vendors = _db.Vendors.Where(n => n.EventId == @event.EventId);
            var resourceMapping = _db.EventResourceMapping.Where(n => n.EventId == @event.EventId);
            var staffMapping = _db.StaffEventMapping.Where(n => n.EventId == @event.EventId);
            var budget = _db.Budgets.Where(n => n.EventId == @event.EventId);
            var checkList = _db.CheckLists.Where(n => n.EventId == @event.EventId);
            var listItems = _db.CheckListItems.Where(n => n.EventId == @event.EventId);
            var invoice = _db.Invoices.Where(n => n.EventId == @event.EventId);
            var geustList = _db.GuestLists.Where(n => n.EventId == @event.EventId);
            var guests = _db.Guests.Where(n => n.EventId == @event.EventId);
            var appointment = _db.Appointments.Where(n => n.EventId == @event.EventId);
            var vendorMapping = _db.EventVendorMappings.Where(n => n.EventId == @event.EventId);
            var notes = _db.Notes.Where(n => n.EventId == @event.EventId);
            var tasks = _db.Tasks.Where(n => n.EventId == @event.EventId);
            _db.Event.Remove(@event);
            foreach (var item in clients)
            {
                //get clients with their login details and their messages
                var users = _db.AppUsers.Where(n => n.ClientId == item.ClientId);
                foreach (var items in users)
                {
                    var messages = _db.Messages.Where(n => n.AppUserId == items.AppUserId);
                    var groupMembers = _db.MessageGroupMembers.Where(n => n.AppUserId == items.AppUserId);
                    var notifications = _db.Notifications.Where(n => n.AppUserId == items.AppUserId);
                    var checklist = _db.PersonalCheckLists.Where(n => n.AppUserId == items.AppUserId);
                    foreach (var itemss in messages)
                    {
                        _db.Messages.Remove(itemss);
                    }
                    foreach (var member in groupMembers)
                    {
                        _db.MessageGroupMembers.Remove(member);
                    }
                    foreach (var notification in notifications)
                    {
                        _db.Notifications.Remove(notification);
                    }
                    foreach (var list in checklist)
                    {
                        var checklistItems = _db.PersonalCheckListItems.Where(n => n.PersonalCheckListId == list.PersonalCheckListId );
                        foreach (var listItem in checklistItems)
                        {
                            _db.PersonalCheckListItems.Remove(listItem);
                        }
                        _db.PersonalCheckLists.Remove(list);
                    }
                    _db.AppUsers.Remove(items);
                }
                _db.Clients.Remove(item);
            }
            foreach (var item in vendors)
                _db.Vendors.Remove(item);
            foreach (var item in resourceMapping)
                _db.EventResourceMapping.Remove(item);
            foreach (var item in staffMapping)
                _db.StaffEventMapping.Remove(item);
            foreach (var item in budget)
                _db.Budgets.Remove(item);
            foreach (var item in checkList)
                _db.CheckLists.Remove(item);
            foreach (var item in listItems)
                _db.CheckListItems.Remove(item);
            //get event invoive and delete the payments and items
            foreach (var item in invoice)
            {
                var payments = _db.InvoicePayments.Where(n => n.InvoiceId == item.InvoiceId);
                var invoiceItems = _db.InvoiceItems.Where(n => n.InvoiceId == item.InvoiceId);

                foreach (var items in payments)
                    _db.InvoicePayments.Remove(items);
                foreach (var itemss in invoiceItems)
                    _db.InvoiceItems.Remove(itemss);
                _db.Invoices.Remove(item);
            }
            foreach (var item in geustList)
                _db.GuestLists.Remove(item);
            foreach (var item in guests)
                _db.Guests.Remove(item);
            foreach (var item in appointment)
                _db.Appointments.Remove(item);
            foreach (var item in vendorMapping)
                _db.EventVendorMappings.Remove(item);
            foreach (var item in notes)
                _db.Notes.Remove(item);
            foreach (var item in tasks)
                _db.Tasks.Remove(item);
            _db.SaveChanges();
            TempData["display"] = "You have successfully deleted the event with all its data!";
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