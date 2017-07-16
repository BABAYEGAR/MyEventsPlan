using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class InvoicePayment : Transport
    {
        public long InvoicePaymentId { get; set; }
        [Required]
        public long? Amount { get; set; }
        [Required]
        [DisplayName("Reference Number")]
        public long? Reference { get; set; }
        [Required]
        [DisplayName("Payment Date")]
        public DateTime PaymentDate { get; set; }
        public long? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
    }
}
