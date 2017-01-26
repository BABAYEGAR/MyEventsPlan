using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using MyEventPlan.Data.DataContext.DataContext;
using MyEventPlan.Data.Service.Enum;

namespace MyEventPlan.Data.Service.Calender
{
    public class CalenderEvent
    {
        public  List<Event.Data.Objects.Entities.Event> LoadAllUserEvents(long? id)
        {
            using (EventDataContext ent = new EventDataContext())
            {
                var rslt = ent.Event.Where(n=>n.EventPlannerId == id && n.StartDate > DateTime.Now);
                List<Event.Data.Objects.Entities.Event> result = new List<Event.Data.Objects.Entities.Event>();
                foreach (var item in rslt)
                {
                    Event.Data.Objects.Entities.Event rec = new Event.Data.Objects.Entities.Event();
                    rec.EventId = item.EventId;
                    rec.StartDate = item.StartDate; 
                    rec.EndDate = item.EndDate; 
                    rec.Name = item.Name;
                    rec.Color = item.Color;
                    result.Add(rec);
                }
                return result;
            }
        }
        public  void UpdateCalendarEvent(int id, string newEventStart, string newEventEnd)
        {
            // EventStart comes ISO 8601 format, eg:  "2000-01-10T10:00:00Z" - need to convert to DateTime
            using (EventDataContext ent = new EventDataContext())
            {
                var rec = ent.Event.FirstOrDefault(s => s.EventId == id);
                if (rec != null)
                {
                    DateTime dateTimeStart = DateTime.Parse(newEventStart, null,
                       DateTimeStyles.RoundtripKind).ToLocalTime(); // and convert offset to localtime
                    rec.StartDate = dateTimeStart;
                    if (!String.IsNullOrEmpty(newEventEnd))
                    {
                        TimeSpan span = DateTime.Parse(newEventEnd, null,
                           DateTimeStyles.RoundtripKind).ToLocalTime() - dateTimeStart;

                    }
                    ent.Entry(rec).State = EntityState.Modified;
                    ent.SaveChanges();
                }
            }
        }
        public bool CreateNewEvent(string title, string newEventStartDate, string newEventEndDate, string newEventStartTime, string newEventEndTime, string color, long budget,
            long plannerId, long type)
        { 
            try
            {
                EventDataContext ent = new EventDataContext();
                Event.Data.Objects.Entities.Event rec = new Event.Data.Objects.Entities.Event
                {
                    Name = title,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Status = EventStausEnum.New.ToString(),
                    Color = color,
                    TargetBudget = budget,
                    EventPlannerId = plannerId,
                    EventTypeId = type,
                    StartTime = newEventStartTime,
                    EndTime = newEventEndTime,
                    CreatedBy = plannerId,
                    LastModifiedBy = plannerId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now
                };

                ent.Event.Add(rec);
                ent.SaveChanges();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
