using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Event.Data.Objects.Entities
{
    public class Appointment :Transport
    {
        public long AppointmentId { get; set; }
        public long EventId { get; set; }
        [ForeignKey("EventId")]
        [Required]
        public virtual Event Event { get; set; }
    }
}
