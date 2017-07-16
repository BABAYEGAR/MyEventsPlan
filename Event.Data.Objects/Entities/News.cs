using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class News :  Transport
    {
        public long NewsId { get; set; }
        [Required]
        public string Content { get; set; }
        public string NewsImage { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public long Likes { get; set; }
        public long Dislike { get; set; }
    }
}
