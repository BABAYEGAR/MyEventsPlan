using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Event : Transport
    {
        public long EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public long EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }
        [Required]
        public long TargetBudget { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string EndTime { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<EventContactMapping> EventContactMapping { get; set; }
    }
}
