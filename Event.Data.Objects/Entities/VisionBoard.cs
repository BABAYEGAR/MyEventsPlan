using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class VisionBoard : Transport
    {
        public long VisionBoardId { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        
        public  IEnumerable<VisionBoardComment> Comments { get; set; }
    }
}
