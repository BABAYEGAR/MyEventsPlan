using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class EventResourceMapping : Transport
    {
        public long EventResourceMappingId { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        public long? ResourceId { get; set; }
        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }
        [Required]
        public long Quantity { get; set; }
    }
}
