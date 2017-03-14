using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
   public class Message : Transport
    { 
        public long MessageId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
        public bool Read { get; set; }
        public string AttachedFile { get; set; }
        public long? Sender { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long? MessageGroupId { get; set; }
        [ForeignKey("MessageGroupId")]
        public MessageGroup MessageGroup { get; set; }
    }
}
