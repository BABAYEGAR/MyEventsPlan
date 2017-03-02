using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class News :  Transport
    {
        public long NewsId { get; set; }
        public string Content { get; set; }
        public string NewsImage { get; set; }
        public long EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        public long Likes { get; set; }
        public long Dislike { get; set; }
    }
}
