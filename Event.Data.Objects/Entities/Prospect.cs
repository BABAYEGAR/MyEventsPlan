using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Prospect : Transport
    {
        public long ProspectId { get; set; }
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
        public long ContactRoleId { get; set; }
        [ForeignKey("ContactRoleId")]
        public virtual ContactRole ContactRole { get; set; }
    }
}
