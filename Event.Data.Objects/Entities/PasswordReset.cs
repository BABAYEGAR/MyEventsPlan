using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class PasswordReset
    {
        public long PasswordResetId { get; set; }
        [DisplayName ("Email")]
        public string Email { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        [DisplayName("Confirm password")]
        [Compare("Password",
             ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public long Code { get; set; }
        public DateTime Date { get; set; }
    }
}
