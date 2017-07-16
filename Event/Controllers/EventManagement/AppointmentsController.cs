using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.AuthenticationManagement;
using MyEventPlan.Data.Service.Calender;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Controllers.EventManagement
{
    public class AppointmentsController : Controller
    {
        private readonly EventDataContext _databaseConnection = new EventDataContext();

        // GET: Appointments
        [SessionExpire]
        public ActionResult Index(long? eventId)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.eventId = eventId;
            var appointments =
                _databaseConnection.Appointments.Where(n => n.EventId == eventId && n.EventPlannerId == loggedinuser.EventPlannerId)
                    .OrderByDescending(n => n.StartDate)
                    .Include(a => a.Event);
            return View(appointments.ToList());
        }

        // GET: Appointments
        [SessionExpire]
        public ActionResult Calendar()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            ViewBag.EventId = new SelectList(_databaseConnection.Event.Where(n => n.EventPlannerId == loggedinuser.EventPlannerId),
                "EventId", "Name");
            ViewBag.ContactId = new SelectList(_databaseConnection.Contacts.Where(n=>n.EventPlannerId == loggedinuser.EventPlannerId).ToList(),
                "ContactId", "Firstname");
            return View();
        }

        //// GET: GetMyAppointments
        public JsonResult GetMyAppointments()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var appointments = new CalenderAppointment().LoadAllUserAppointments(loggedinuser?.EventPlannerId);
                var appointmentList = from e in appointments
                    select new
                    {
                        id = e.AppointmentId,
                        title = e.Name,
                        start = e.StartDate,
                        location = e.Location,
                        note = e.Notes,
                        end = e.EndDate,
                        allDay = false,
                        startTime = e.StartTime,
                        endTime = e.EndTime
                    };
                var rows = appointmentList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // POST: Appointment/UpdateCalendarAppointment/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult UpdateCalendarAppointment(FormCollection collectedValues)
        {
            var appointmentId = Convert.ToInt64(collectedValues["appointmentId"]);
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var calendarAppointment = _databaseConnection.Appointments.Find(appointmentId);
            if (calendarAppointment != null)
            {
                calendarAppointment.DateLastModified = DateTime.Now;
                if (loggedinuser != null) calendarAppointment.LastModifiedBy = loggedinuser.AppUserId;
                calendarAppointment.StartDate = Convert.ToDateTime(collectedValues["StartDate"]);
                calendarAppointment.Notes = collectedValues["Notes"];
                calendarAppointment.Name = collectedValues["Name"];
                calendarAppointment.Location = collectedValues["Location"];
                calendarAppointment.EndDate = Convert.ToDateTime(collectedValues["EndDate"]);
                calendarAppointment.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                calendarAppointment.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                _databaseConnection.Entry(calendarAppointment).State = EntityState.Modified;
            }
            _databaseConnection.SaveChanges();
            return RedirectToAction("Calendar");
        }

        [SessionExpire]
        public void UpdateEventAppoitments(int id, string newEventStart, string newEventEnd)
        {
            new CalenderAppointment().UpdateCalendarEventAppoitment(id, newEventStart, newEventEnd);
        }

        // GET: Appointments/Details/5
        [SessionExpire]
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _databaseConnection.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            return View(appointment);
        }

        [SessionExpire]
        public bool CreateNewAppointment(string title,string reason ,string newEventStartDate, string newEventEndDate,
            long appUserId,string[] contacts, string location, string note,
            long plannerId, long? eventId,string setReminder,long? reminderLength
            ,string reminderLengthType, string reminderRepeat,string sendEmailReminder)
        {
            try
            {
                new CalenderAppointment().CreateNewAppointment(title,reason, newEventStartDate, newEventEndDate, appUserId,
                    location, note,
                    plannerId, eventId,contacts,setReminder,reminderLength
                    , reminderLengthType, reminderRepeat, sendEmailReminder);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // GET: Appointments/Create
        [SessionExpire]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Create(
            [Bind(Include = "AppointmentId,Name,EventId,StartDate,EndDate,Location,Notes")] Appointment appointment,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = Session["role"] as Role;
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            if (ModelState.IsValid)
            {
                if (role != null && loggedinuser != null && role.Name == "Event Planner")
                {
                    appointment.CreatedBy = loggedinuser.AppUserId;
                    appointment.DateCreated = DateTime.Now;
                    appointment.DateLastModified = DateTime.Now;
                    appointment.LastModifiedBy = loggedinuser.AppUserId;
                    if (events != null) appointment.EventId = events.EventId;
                    appointment.EventPlannerId = loggedinuser.EventPlannerId;
                    appointment.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    appointment.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Appointments.Add(appointment);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added an appointment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = appointment.EventId});
            }
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult CreateAppointmentFromEvent(
            [Bind(Include = "AppointmentId,Name,StartDate,EndDate,Location,Notes,For")] Appointment appointment,
            FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = Session["role"] as Role;
            var events = Session["event"] as Event.Data.Objects.Entities.Event;
            if (ModelState.IsValid)
            {
                if (role != null && loggedinuser != null && role.Name == "Event Planner")
                {
                    appointment.CreatedBy = loggedinuser.AppUserId;
                    appointment.DateCreated = DateTime.Now;
                    appointment.DateLastModified = DateTime.Now;
                    appointment.LastModifiedBy = loggedinuser.AppUserId;
                    appointment.EventPlannerId = loggedinuser.EventPlannerId;
                    appointment.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToShortTimeString();
                    appointment.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToShortTimeString();
                    appointment.ContactId = null;
                    appointment.ReminderLength = null;
                    appointment.ReminderLengthType = null;
                    appointment.SendEmailReminder = false;
                    appointment.SendTextMessageReminder = false;
                    appointment.SetReminder = false;
                    
                    if (events != null) appointment.EventId = events.EventId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Appointments.Add(appointment);
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully added an appointment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Details", "Events", new {id = appointment.EventId});
            }
            TempData["display"] = "There was an issue creating a new appointment!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Details", "Events", new { id = appointment.EventId });
        }

        // GET: Appointments/Edit/5
        [SessionExpire]
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _databaseConnection.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult Edit(
            [Bind(
                Include =
                    "AppointmentId,Name,EventId,StartDate,EndDate,Location,Notes,EventPlannerId,DateCreated,CreatedBy,EventId")]
            Appointment appointment, FormCollection collectedValues)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                appointment.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    appointment.LastModifiedBy = loggedinuser.AppUserId;
                    appointment.StartTime = Convert.ToDateTime(collectedValues["StartDate"]).ToString("HH:mm");
                    appointment.EndTime = Convert.ToDateTime(collectedValues["EndDate"]).ToString("HH:mm");
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _databaseConnection.Entry(appointment).State = EntityState.Modified;
                _databaseConnection.SaveChanges();
                TempData["display"] = "You have successfully modified an appointment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index", new {eventId = appointment.EventId});
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [SessionExpire]
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _databaseConnection.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SessionExpire]
        public ActionResult DeleteConfirmed(long id)
        {
            var appointment = _databaseConnection.Appointments.Find(id);
            var eventId = appointment.EventId;
            _databaseConnection.Appointments.Remove(appointment);
            _databaseConnection.SaveChanges();
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