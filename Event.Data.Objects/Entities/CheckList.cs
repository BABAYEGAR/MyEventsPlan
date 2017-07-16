using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
   public class CheckList : Transport
    {
        public  long CheckListId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { get; set; }
        public long EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        public IEnumerable<CheckListItem> CheckListItems { get; set; }
    }
}
