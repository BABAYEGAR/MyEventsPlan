using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class EventContactMapping
    {
        public long EventContactMappingId { get; set; }
        public long EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        public long ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
