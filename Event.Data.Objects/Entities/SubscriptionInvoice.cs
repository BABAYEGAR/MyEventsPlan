using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class SubscriptionInvoice : Transport
    {
        public long SubscriptionInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public long PackageId { get; set; }
        public Package Package { get; set; }
        public long AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
    }
}
