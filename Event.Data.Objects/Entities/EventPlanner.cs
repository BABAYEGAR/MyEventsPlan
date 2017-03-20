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
        public string Firstname { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is compulsory")]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
       
        public string Mobile { get; set; }
        public string Type { get; set; }
        [DisplayName("Business Name")]
        public string BusinessName { get; set; }
        [DisplayName("Business Contact")]
        public string BusinessContact { get; set; }
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
        public IEnumerable<AppUser> AppUsers { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Prospect> Prospects { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<EventVendorMapping> EventVendorMappings { get; set; }
        public IEnumerable<StaffEventMapping> StaffEventMapping { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<ContactRole> ContactRoles { get; set; }
        public IEnumerable<GuestList> GuestLists { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<News> Newses { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}
