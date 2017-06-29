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
    public class MealChoice
    {
        public long MealChoiceId { get; set; }
        [DisplayName("Meal Choice")]
        [Required]
        public string Choice { get; set; }
        public long GuestListId { get; set; }
        [ForeignKey("GuestListId")]
        public GuestList GuestList { get; set; }
    }
}
