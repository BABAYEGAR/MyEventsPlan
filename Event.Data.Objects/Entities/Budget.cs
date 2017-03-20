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
    public class Budget : Transport
    {
        public long BudgetId { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Required]
        [DisplayName("Estimated Amount")]
        public long EstimatedAmount { get; set; }
        [Required]
        [DisplayName("Negotiated Amount")]
        public long NegotiatedAmount { get; set; }
        [Required]
        [DisplayName("Actual Amount")]
        public long ActualAmount { get; set; }
        [Required]
        [DisplayName("Paid Till Date")]
        public long PaidTillDate { get; set; }
        [DisplayName("Amount Still Due")]
        public long AmountStillDue { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
    }
}
