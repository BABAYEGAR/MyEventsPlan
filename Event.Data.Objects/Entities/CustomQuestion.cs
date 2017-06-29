using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class CustomQuestion
    {
        public long CustomQuestionId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        public long GuestListId { get; set; }
        [ForeignKey("GuestListId")]
        public GuestList GuestList { get; set; }
    }
}
