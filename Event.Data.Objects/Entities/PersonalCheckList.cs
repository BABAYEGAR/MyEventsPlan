using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
   public class PersonalCheckList : Transport
    {
        public  long PersonalCheckListId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public IEnumerable<PersonalCheckListItem> PersonalCheckListItems { get; set; }
    }
}
