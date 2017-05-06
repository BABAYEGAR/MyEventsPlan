using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class AppUser :Transport
    {
        public long AppUserId { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("[a-zA-Z ]*$")]
        public string Firstname { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        public string Lastname { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
       [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "This field is does not support more than 100 characters")]
        [RegularExpression("^[0-9]*$")]
        public string Mobile { get; set; }
        [Required]
        public string Password { get; set; }
        public long? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public long? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        public long? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public string ProfileImage { get; set; }
        public string Status { get; set; }
        [DisplayName("Background Color")]
        public string BackgroundColor { get; set; }
        public bool Verified { get; set; }
        public  IEnumerable<Message> Messages { get; set; }
        public  IEnumerable<Setting> Settings { get; set; }
        public IEnumerable<SubscriptionInvoice> SubscriptionInvoices { get; set; }
        public IEnumerable<VendorPackageSetting> VendorPackageSettings { get; set; }
        public string DisplayName
     => Firstname + " " + Lastname;


    }
}
