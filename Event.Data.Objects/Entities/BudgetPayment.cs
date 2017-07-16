using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class BudgetPayment
    {
        public long BudgetPaymentId { get; set; }
        public long AmountPaid { get; set; }
        public DateTime DatePaid { get; set; }
        public long BudgetId { get; set; }
        [ForeignKey("BudgetId")]
        public Budget Budget { get; set; }
    }
}
