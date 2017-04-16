using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class PasswordReset
    {
        public long PasswordResetId { get; set; }
        [DisplayName ("Email")]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
        [PasswordPropertyText]
        [Compare("Password")]
        [Required]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public long Code { get; set; }
        public DateTime Date { get; set; }
    }
}
