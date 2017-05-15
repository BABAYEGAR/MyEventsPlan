using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class SubscriptionInvoice : Transport
    {
        public long SubscriptionInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public long PackageId { get; set; }
        public EventPlannerPackage Package { get; set; }
        public long? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public long? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
    }
}
