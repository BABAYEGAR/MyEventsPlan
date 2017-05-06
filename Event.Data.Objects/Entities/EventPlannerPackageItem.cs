using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class EventPlannerPackageItem : Transport
    {
        public long EventPlannerPackageItemId { get; set; }
        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Required]
        public long Amount { get; set; }
        public long EventPlannerPackageId { get; set; }
        [ForeignKey("EventPlannerPackageId")]
        public EventPlannerPackage EventPlannerPackage { get; set; }
    }
}
