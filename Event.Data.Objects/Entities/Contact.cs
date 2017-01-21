using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Contact :Transport
    {
        public long ContactId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        public long? ContactRoleId { get; set; }
        [ForeignKey("ContactRoleId")]
        public virtual ContactRole ContactRole { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Prospect> Prospects { get; set; }
        public IEnumerable<EventContactMapping> EventContactMapping { get; set; }
    }
}
