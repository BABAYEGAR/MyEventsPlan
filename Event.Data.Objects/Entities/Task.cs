using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Task : Transport
    {
        public long TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Status { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }
        public long EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
