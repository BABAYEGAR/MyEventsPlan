using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Client : Transport
    {
        public long ClientId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

        public long? EventPlannerId { get; set; }

        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }

        public long? EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}