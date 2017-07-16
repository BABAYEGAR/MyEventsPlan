using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    Event.Data.Objects.Entities.Event rec = new Event.Data.Objects.Entities.Event
                    {
                        EventId = item.EventId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Name = item.Name,
                        Color = item.Color,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime
                    };


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
                    rec.StartDate = Convert.ToDateTime(newEventStart);
                    rec.EndDate = Convert.ToDateTime(newEventEnd);
                    ent.Entry(rec).State = EntityState.Modified;
                    ent.SaveChanges();
                }
            }
        }
        public bool CreateNewEvent(string title, string newEventStartDate, string newEventEndDate,long appUserId, string color, string budget,
            long plannerId, long type,string eventDate)
        {
            try
            {
                EventDataContext ent = new EventDataContext();
                Event.Data.Objects.Entities.Event rec = new Event.Data.Objects.Entities.Event
                {
                    Name = title,
                    StartDate = Convert.ToDateTime(newEventStartDate),
                    EndDate = Convert.ToDateTime(newEventEndDate),
                    Status = EventStausEnum.New.ToString(),
                    Color = color,
                    TargetBudget = budget.Replace(",", ""),
                     EventPlannerId = plannerId,
                    EventTypeId = type,
                    StartTime = Convert.ToDateTime(newEventStartDate).ToShortTimeString(),
                    EndTime = Convert.ToDateTime(newEventEndDate).ToShortTimeString(),
                    CreatedBy = appUserId,
                    LastModifiedBy = appUserId,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    EventDate = Convert.ToDateTime(eventDate)
                };

                ent.Event.Add(rec);
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
