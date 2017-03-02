using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class NewsAction :  Transport
    {
        public long NewsActionId { get; set; }
        public string Action { get; set; }
        public long NewsId { get; set; }
        [ForeignKey("NewsId")]
        public News News { get; set; }
        public long AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}
