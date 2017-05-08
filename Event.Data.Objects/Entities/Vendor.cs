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
        public string ConfirmPassword { get; set; }
        [Required]
        public string Address { get; set; }
        [DisplayName("Facebook Page Link")]
        public string FacebookPage { get; set; }
        [DisplayName("Twitter Page Link")]
        public string TwitterPage { get; set; }
        [DisplayName("Instagram Page Link")]
        public string InstagramPage { get; set; }
        [DisplayName("Google Plus Page Link")]
        public string GooglePlusPage { get; set; }
        [DisplayName("Youtube Page Link")]
        public string YoutubePage { get; set; }
        public string Website { get; set; }
        public string About { get; set; }
        public string Logo { get; set; }
        public long AveragePrice { get; set; }
        public long PricingDetails { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
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
        public  IEnumerable<VendorPackageSetting> VendorPackageSettings { get; set; }
        public  IEnumerable<AppUser> AppUsers { get; set; }
        public  IEnumerable<VendorReview> VendorReviews { get; set; }
        public  IEnumerable<VendorEnquiry> VendorEnquiries { get; set; }
        public  IEnumerable<VendorImage> VendorImages { get; set; }
    }
}
