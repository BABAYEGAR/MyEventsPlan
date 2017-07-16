using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class VendorPackageItem : Transport
    {
        public long VendorPackageItemId { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        public long VendorPackageId { get; set; }
        [ForeignKey("VendorPackageId")]
        public VendorPackage VendorPackage { get; set; }
    }
}
