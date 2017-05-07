using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class VendorPackageSetting : Transport
    {
        public long VendorPackageSettingId { get; set; }
        public long Amount { get; set; }
        public long VendorPackageId { get; set; }
        [ForeignKey("VendorPackageId")]
        public VendorPackage VendorPackage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public long VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
