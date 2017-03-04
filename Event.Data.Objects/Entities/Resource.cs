using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Resource : Transport
    {
        public long ResourceId { get; set; }
        public string Name { get; set; }
        public long Quantity { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
    }
}
