using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Note :  Transport
    {
        public long NoteId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public long EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
