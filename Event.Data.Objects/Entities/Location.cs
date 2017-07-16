using System.Collections.Generic;

namespace Event.Data.Objects.Entities
{
    public class Location : Transport
    {
        public long LocationId { get; set; }
        public string Name { get; set; }
        public IEnumerable<EventPlanner> EventPlanners { get; set; }
    }
}
