using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Vendor : Transport
    {
        public long VendorId { get; set; }
        [Required]
        public string Name { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        [PasswordPropertyText]
        [DisplayName("Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string About { get; set; }
        public string ImageOne { get; set; }
         public string ImageTwo { get; set; }
        public string ImageThree { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        [DisplayName("Business Name")]
        public string BusinessName { get; set; }
        [Required]
        [DisplayName("Business Contact")]
        public string BusinessContact { get; set; }
        [Required]
        [DisplayName("Vendor Service")]
        public  long? VendorServiceId { get; set; }
        [ForeignKey("VendorServiceId")]
        public virtual VendorService VendorService { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public long? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public  IEnumerable<EventVendorMapping> EventVendorMappings { get; set; }
        public  IEnumerable<AppUser> AppUsers { get; set; }
    }
}
