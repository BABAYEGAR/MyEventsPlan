using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Staff : Transport
    {
        public long StaffId { get; set; }
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
        [PasswordPropertyText]
        public string Password { get; set; }
        public string Status { get; set; }
        public long? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [DisplayName("Event Planner")]
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<StaffEventMapping> StaffEventMapping { get; set; }
        public string DisplayName
=> Firstname + " " + Lastname;
    }
}
