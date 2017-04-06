using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerPackage : Transport
    {
        public long EventPlannerPackageId { get; set; }
        public string Status { get; set; }
        public long SubscribedEvent { get; set; }
        public long AllowedEvent { get; set; }
        public long PackageId { get; set; }
        [ForeignKey("PackageId")]
        public Package Package { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
