using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerPackage : Transport
    {
        public long EventPlannerPackageId { get; set; }
        [Required]
        [DisplayName("Package Name")]
        public string PackageName { get; set; }
        [Required]
        [DisplayName("Package Amount")]
        public long? Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Package Grade")]
        public string PackageGrade { get; set; }
        [Required]
        [DisplayName("Maximum Number Of Events")]
        public long MaximumEvents { get; set; }
        public IEnumerable<EventPlannerPackageSetting> EventPlannerPackageSettings { get; set; }
        public IEnumerable<EventPlannerPackageItem> EventPlannerPackageItems { get; set; }
    }
}
