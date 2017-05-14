using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerReview : Transport
    {
        public long EventPlannerReviewId { get; set; }
        [Required]
        public string ReviewerName { get; set; }
        [Required]
        public string ReviewerEmail { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewBody { get; set; }
        public long? Rating { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public  EventPlanner EventPlanner { get; set; }
        
    }
}
