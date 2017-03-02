using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
   public class CheckListItem : Transport
    {
        public  long CheckListItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public bool Checked { get; set; }
        public long EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public long CheckListId { get; set; }
        [ForeignKey("CheckListId")]
        public CheckList CheckList { get; set; }
    }
}
