using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Calender;
using MyEventPlan.Data.Service.Enum;
using Event = Event.Data.Objects.Entities.Event;

namespace MyEventPlan.Controllers.EventManagement
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentDataContext _db = new AppointmentDataContext();

        // GET: Appointments
        public ActionResult Index(long? eventId)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var appointments = _db.Appointments.Where(n=>n.EventId == eventId && n.EventPlannerId == loggedinuser.EventPlannerId ).Include(a => a.Event);
            return View(appointments.ToList());
        }
        // GET: Appointments
        public ActionResult Calendar()
        {
            return View();
        }
        //// GET: GetMyAppointments
        public JsonResult GetMyAppointments()
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var appointments = new CalenderAppointment().LoadAllUserAppointments(loggedinuser?.EventPlannerId);
            global::Event.Data.Objects.Entities.Event appoitmentEvent = null;
            foreach (var item in appointments)
            {
                appoitmentEvent = _db.Event.Find(item.EventId);

                var appointmentList = from e in appointments
                    select new
                    {
                        id = e.AppointmentId,
                        title =
                        appoitmentEvent.Name + Environment.NewLine + e.Name + " " +
                        (Convert.ToDateTime(e.EndTime) - Convert.ToDateTime(e.StartTime)) + " mins",
                        start = e.StartDate,
                        end = e.EndDate,
                        allDay = false
                    };
                var rows = appointmentList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            return Json(appointments.ToArray(), JsonRequestBehavior.AllowGet);
        }
        public void UpdateEventAppoitments(int id, string newEventStart, string newEventEnd)
        {
            new CalenderAppointment().UpdateCalendarEventAppoitment(id, newEventStart, newEventEnd);
        }
        // GET: Appointments/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "AppointmentId,Name,EventId,StartDate,StartTime,EndDate,EndTime,Location,Notes")] Appointment appointment)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            var role = Session["role"] as Role;
            if (ModelState.IsValid)
            {
                if ((role != null) && (loggedinuser != null) && (role.Name == "Event Planner"))
                {
                    appointment.CreatedBy = loggedinuser.AppUserId;
                    appointment.DateCreated = DateTime.Now;
                    appointment.DateLastModified = DateTime.Now;
                    appointment.LastModifiedBy = loggedinuser.AppUserId;
                    appointment.EventPlannerId = loggedinuser.EventPlannerId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
                TempData["appoint"] = "You have successfully booked an appointment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name", appointment.EventId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name", appointment.EventId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "AppointmentId,Name,EventId,StartDate,StartTime,EndDate,EndTime,Location,Notes,EventPlannerId")] Appointment appointment)
        {
            var loggedinuser = Session["myeventplanloggedinuser"] as AppUser;
            if (ModelState.IsValid)
            {
                appointment.DateLastModified = DateTime.Now;
                if (loggedinuser != null)
                {
                    appointment.LastModifiedBy = loggedinuser.AppUserId;
                }
                else
                {
                    TempData["login"] = "Your session has expired, Login again!";
                    TempData["notificationtype"] = NotificationType.Info.ToString();
                    return RedirectToAction("Login", "Account");
                }
                _db.Entry(appointment).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["appoint"] = "You have successfully modified an appointment!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(_db.Event, "EventId", "Name", appointment.EventId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appointment = _db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var appointment = _db.Appointments.Find(id);
            _db.Appointments.Remove(appointment);
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