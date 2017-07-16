using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Notification : Transport
    {
        public long NotificationId { get; set; }
        public string Message { get; set; }
        public string NotificationType { get; set; }
        public bool Read { get; set; }
        public long NotificationKey { get; set; }
        public long AppUserId  { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser  { get; set; }
    }
}
