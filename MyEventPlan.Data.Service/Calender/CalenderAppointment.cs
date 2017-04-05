using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Data.Service.Calender
{
    public class CalenderAppointment
    {
        public  List<Event.Data.Objects.Entities.Appointment> LoadAllEventsAppointments(long? id)
        {
            using (AppointmentDataContext ent = new AppointmentDataContext())
            {
                var rslt = ent.Appointments.Where(n=>n.EventPlannerId == id && n.StartDate >= DateTime.Now);
                List<Event.Data.Objects.Entities.Appointment> result = new List<Event.Data.Objects.Entities.Appointment>();
                foreach (var item in rslt)
                {
                    Event.Data.Objects.Entities.Appointment rec = new Event.Data.Objects.Entities.Appointment
                    {
                        EventId = item.EventId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Name = item.Name,
                        Notes = item.Notes,
                        Location = item.Location,
                        EventPlannerId = id,
                        AppointmentId = item.AppointmentId
                    };


                    result.Add(rec);
                }
                return result;
            }
        }
        public List<Event.Data.Objects.Entities.Appointment> LoadAllUserAppointments(long? id)
        {
            using (AppointmentDataContext ent = new AppointmentDataContext())
            {
                var rslt = ent.Appointments.Where(n => n.EventPlannerId == id).Include(n=>n.Event);
                List<Event.Data.Objects.Entities.Appointment> result = new List<Event.Data.Objects.Entities.Appointment>();
                foreach (var item in rslt)
                {
                    Event.Data.Objects.Entities.Appointment rec = new Event.Data.Objects.Entities.Appointment
                    {
                        EventId = item.EventId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Name = item.Name,
                        Notes = item.Notes,
                        Location = item.Location,
                        EventPlannerId = id,
                        AppointmentId = item.AppointmentId,
                        Event = item.Event
                    };
                    result.Add(rec);
                }
                return result;
            }
        }
        public  void UpdateCalendarEventAppoitment(int id, string newEventStart, string newEventEnd)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using (AppointmentDataContext ent = new AppointmentDataContext())
            {
                var rec = ent.Appointments.FirstOrDefault(s => s.AppointmentId == id);
                if (rec != null)
                {
                    rec.StartDate = Convert.ToDateTime(newEventStart);
                    rec.EndDate = Convert.ToDateTime(newEventEnd);
                    ent.Entry(rec).State = EntityState.Modified;
                    ent.SaveChanges();
                }
            }
        }
        public bool CreateNewAppointment(string title, string newEventStartDate, string newEventEndDate, long appUserId, string location, string note,
       long plannerId,long eventId)
        {
            try
            {
                AppointmentDataContext ent = new AppointmentDataContext();
                Event.Data.Objects.Entities.Appointment rec = new Event.Data.Objects.Entities.Appointment
                {
                    Name = title,
                    StartDate = Convert.ToDateTime(newEventStartDate),
                    EndDate = Convert.ToDateTime(newEventEndDate),
                    Location = location,
                    EventPlannerId = plannerId,
                    Notes = note,
                    StartTime = Convert.ToDateTime(newEventStartDate).ToShortTimeString(),
                    EndTime = Convert.ToDateTime(newEventEndDate).ToShortTimeString(),
                    CreatedBy = appUserId,
                    LastModifiedBy = appUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    EventId = eventId
                };

                ent.Appointments.Add(rec);
                ent.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
