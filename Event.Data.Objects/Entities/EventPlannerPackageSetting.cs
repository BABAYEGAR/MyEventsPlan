using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerPackageSetting : Transport
    {
        public long EventPlannerPackageSettingId { get; set; }
        public string Status { get; set; }
        public long SubscribedEvent { get; set; }
        public long AllowedEvent { get; set; }
        public long EventPlannerPackageId { get; set; }
        [ForeignKey("EventPlannerPackageId")]
        public EventPlannerPackage EventPlannerPackage { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
