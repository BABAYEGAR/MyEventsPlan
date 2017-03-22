using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Invoice : Transport
    {
        public long InvoiceId { get; set; }
        [DisplayName("Invoice Name")]
        [Required]
        public string InvoiceName { get; set; }
        [DisplayName("Invoice Number")]
        [Required]
        public long InvoiceNumber { get; set; }
        [DisplayName("Due Date")]
        [Required]
        public DateTime DueDate { get; set; }
        public long ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }
        public IEnumerable<InvoicePayment> InvoicePayments { get; set; }

    }
}
