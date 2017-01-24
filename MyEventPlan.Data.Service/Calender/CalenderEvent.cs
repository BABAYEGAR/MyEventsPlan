using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Event.Data.Objects.Entities;
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
                var rslt = ent.Event.Where(n=>n.EventPlannerId == id);
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
    }
}
