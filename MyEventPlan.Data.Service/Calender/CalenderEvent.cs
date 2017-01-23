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
        //private readonly EventDataContext _ent = new EventDataContext();
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
                    //rec.StatusColor = item.StatusColor(typeof(DescriptionAttribute), false);
                    result.Add(rec);
                }
                return result;
            }
        }
        public static string GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = System.Enum.GetNames(type).Where(f => f.Equals(value,
            StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();
            if (name == null)
            {
                return string.Empty;
            }
            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }
    }
}
