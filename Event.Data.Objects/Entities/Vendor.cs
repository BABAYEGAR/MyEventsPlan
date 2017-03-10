using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Vendor : Transport
    {
        public long VendorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string Image { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string BusinessContact { get; set; }
        [Required]
        public  long? VendorServiceId { get; set; }
        [ForeignKey("VendorServiceId")]
        public virtual VendorService VendorService { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public long? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public  IEnumerable<EventVendorMapping> EventVendorMappings { get; set; }
    }
}
