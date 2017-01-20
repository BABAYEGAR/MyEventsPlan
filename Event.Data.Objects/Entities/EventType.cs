using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class EventType : Transport
    {
        public long EventTypeId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
