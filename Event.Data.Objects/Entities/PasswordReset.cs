using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class PasswordReset
    {
        public long PasswordResetId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Compare("Password",
             ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public long Code { get; set; }
        public DateTime Date { get; set; }
    }
}
