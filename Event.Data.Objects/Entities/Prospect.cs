using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Prospect : Transport
    {
        public long ProspectId { get; set; }
        [DisplayName("Event Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Event Color")]
        public string Color { get; set; }
        [Required]
        [DisplayName("Event Type")]
        public long? EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }
        [Required]
        [DisplayName("Target Budget")]
        public long TargetBudget { get; set; }
        [Required]
        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DisplayName("Start Time")]
        public string StartTime { get; set; }
        [Required]
        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        [DisplayName("End Time")]
        public string EndTime { get; set; }
        [DisplayName("Event Planner")]
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public string Status { get; set; }

    }
}
