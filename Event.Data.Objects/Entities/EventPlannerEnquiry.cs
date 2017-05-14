using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerEnquiry : Transport
    {
        public long EventPlannerEnquiryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string Note { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
    }
}
