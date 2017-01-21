using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class EventType : Transport
    {
        public long EventTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
