using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
   public class PersonalCheckListItem : Transport
    {
        public  long PersonalCheckListItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Checked { get; set; }
        public long PersonalCheckListId { get; set; }
        [ForeignKey("PersonalCheckListId")]
        public PersonalCheckList PersonalCheckList { get; set; }
    }
}
