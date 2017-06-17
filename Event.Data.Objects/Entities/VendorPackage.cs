using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class VendorPackage : Transport
    {
        public long VendorPackageId { get; set; }
        [Required]
        [DisplayName("Package Name")]
        public string PackageName { get; set; }
        [Required]
        [DisplayName("Package Amount")]
        public long? Amount { get; set; }
        [Required]
        [DisplayName("Package Grade")]
        public string PackageGrade { get; set; }
        [Required]
        public string Description { get; set; }
        public IEnumerable<VendorPackageSetting> VendorPackageSettings { get; set; }
        public IEnumerable<VendorPackageItem> VendorPackageItems { get; set; }
    }
}
