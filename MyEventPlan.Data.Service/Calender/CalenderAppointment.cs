using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Event.Data.Objects.Entities;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Data.Service.Calender
{
    public class CalenderAppointment
    {
        public  List<Event.Data.Objects.Entities.Appointment> LoadAllEventsAppointments(long? id)
        {
            using (EventDataContext databaseConnection = new EventDataContext())
            {
                var rslt = databaseConnection.Appointments.Where(n=>n.EventPlannerId == id && n.StartDate >= DateTime.Now);
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
            using (EventDataContext databaseConnection = new EventDataContext())
            {
                var rslt = databaseConnection.Appointments.Where(n => n.EventPlannerId == id).Include(n=>n.Event);
                List<Event.Data.Objects.Entities.Appointment> result = new List<Event.Data.Objects.Entities.Appointment>();
                foreach (var item in rslt)
                {
                    Event.Data.Objects.Entities.Appointment rec = new Event.Data.Objects.Entities.Appointment
                    {
                        EventId = item.EventId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
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
        public  void UpdateCalendarEventAppoitment(int id, string newEventStart, string newEventEnd)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using (EventDataContext databaseConnection = new EventDataContext())
            {
                var rec = databaseConnection.Appointments.FirstOrDefault(s => s.AppointmentId == id);
                if (rec != null)
                {
                    rec.StartDate = Convert.ToDateTime(newEventStart);
                    rec.EndDate = Convert.ToDateTime(newEventEnd);
                    databaseConnection.Entry(rec).State = EntityState.Modified;
                    databaseConnection.SaveChanges();
                }
            }
        }
        public bool CreateNewAppointment(string title,string reason, string newEventStartDate, string newEventEndDate, long appUserId, string location, string note,
       long plannerId,long? eventId,string[] contacts, string setReminder, long? reminderLength
            , string reminderLengthType, string reminderRepeat, string sendEmailReminder)
        {
            try
            {
                EventDataContext databaseConnection = new EventDataContext();
                Event.Data.Objects.Entities.Appointment rec = new Event.Data.Objects.Entities.Appointment
                {
                    Name = title,
                    StartDate = Convert.ToDateTime(newEventStartDate),
                    EndDate = Convert.ToDateTime(newEventEndDate),
                    Location = location,
                    EventPlannerId = plannerId,
                    For = typeof(AppointmentType).GetEnumName(int.Parse(reason)),
                    Notes = note,
                    StartTime = Convert.ToDateTime(newEventStartDate).ToShortTimeString(),
                    EndTime = Convert.ToDateTime(newEventEndDate).ToShortTimeString(),
                    CreatedBy = appUserId,
                    LastModifiedBy = appUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    EventId = eventId,
                    ReminderLength = reminderLength,
                    ReminderRepeat = reminderRepeat,
                    ReminderLengthType = reminderLengthType,
                    SetReminder = Convert.ToBoolean(setReminder),
                    SendEmailReminder = Convert.ToBoolean(sendEmailReminder),
                    SendTextMessageReminder = false

                };
                var contactList = contacts;
                databaseConnection.Appointments.Add(rec);
                databaseConnection.SaveChanges();
                foreach (var item in contactList)
                {
                    var mapping = new AppointmentContactMapping();
                    var contact = databaseConnection.Contacts.Find(Convert.ToInt64(item));
                    if (contact != null) mapping.ContactId = contact.ContactId;
                    mapping.AppointmentId = rec.AppointmentId;
                    databaseConnection.AppointmentContactMapping.Add(mapping);
                    databaseConnection.SaveChanges();

                }
             

                //var appointContactMapping = new AppointmentContactMapping();
                //appointContactMapping.ContactId
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

    }
}
