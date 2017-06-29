using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class VisionBoardComment : Transport
    {
        public long VisionBoardCommentId { get; set; }
        public string Comment { get; set; }
        public long VisionBoardId { get; set; }
        [ForeignKey("VisionBoardId")]
        public VisionBoard VisionBoard { get; set; }
    }
}
