using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
   public class MessageGroupMember : Transport
    {
        public long MessageGroupMemberId { get; set; }
        public long MessageGroupId { get; set; }
        [ForeignKey("MessageGroupId")]
        public MessageGroup MessageGroup { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
