using System.Collections.Generic;

namespace Event.Data.Objects.Entities
{
   public class MessageGroup :  Transport
    {
        public long MessageGroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Message> Messages { get; set; }
        public IEnumerable<MessageGroupMember> MessageGroupMembers { get; set; }
    }
}
