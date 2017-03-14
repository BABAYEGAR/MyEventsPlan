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
                        EventPlannerId = id, AppointmentId = item.AppointmentId
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
                        Name = item.Name
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
                    DateTime dateTimeStart = DateTime.Parse(newEventStart, null,
                       DateTimeStyles.RoundtripKind).ToLocalTime(); // and convert offset to localtime
                    TimeSpan timeDifference = rec.EndDate - rec.StartDate;
                    rec.EndDate = dateTimeStart.Add(timeDifference);

                    rec.StartDate = dateTimeStart;
                   
                    ent.Entry(rec).State = EntityState.Modified;
                    ent.SaveChanges();
                }
            }
        }
      
    }
}
