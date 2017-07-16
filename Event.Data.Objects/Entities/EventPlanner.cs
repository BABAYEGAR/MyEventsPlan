using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class EventPlanner
    {
        public long EventPlannerId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is compulsory")]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
       
        public string Mobile { get; set; }
        public string Type { get; set; }
        [Required]
        [PasswordPropertyText]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        [DisplayName("Confirm Pasword")]
        [Required]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }
        public long? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [DisplayName("Location")]
        public long? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
        public long? AverageRating { get; set; }
        [DisplayName("Facebook Url")]
        public string FacebookPage { get; set; }
        [DisplayName("Twitter Url")]
        public string TwitterPage { get; set; }
        [DisplayName("Instagram Url")]
        public string InstagramPage { get; set; }
        [DisplayName("Google Plus Url")]
        public string GooglePlusPage { get; set; }
        [DisplayName("Youtube Url")]
        public string YoutubePage { get; set; }
        public string Website { get; set; }
        public string About { get; set; }
        public string Logo { get; set; }
        [DisplayName("Minimum Price")]
        public long? MinimumPrice { get; set; }
        [DisplayName("Maximum Price")]
        public long? MaximumPrice { get; set; }
        [DisplayName("Pricing Details")]
        public string PricingDetails { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Prospect> Prospects { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<EventVendorMapping> EventVendorMappings { get; set; }
        public IEnumerable<StaffEventMapping> StaffEventMapping { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<News> Newses { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public IEnumerable<EventPlannerPackageSetting> EventPlannerPackageSettings { get; set; }
        public IEnumerable<SubscriptionInvoice> SubscriptionInvoices { get; set; }
        public IEnumerable<EventPlannerReview> EventPlannerReviews { get; set; }
        public IEnumerable<EventPlannerEnquiry> EventPlannerEnquiries { get; set; }
        public IEnumerable<ToDo> ToDos { get;set; }
    }
}
