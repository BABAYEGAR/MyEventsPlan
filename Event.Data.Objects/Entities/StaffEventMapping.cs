using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class StaffEventMapping  : Transport
    {
        public long StaffEventMappingId { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        [Required]
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
    }
}
