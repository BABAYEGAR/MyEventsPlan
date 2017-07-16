using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class ToDo
    {
        public long ToDoId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Due Date")]
        public DateTime DueDate { get; set; }
        [Required]
        [DisplayName("Event")]
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        [Required]
        [DisplayName("Assign To")]
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public Contact Contact { get; set; }
        [Required]
        [DisplayName("To-Do For?")]
        public long? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public EventPlanner EventPlanner { get; set; }
        [DisplayName("Set Reminder")]
        public bool SetReminder { get; set; }
        public string Notes { get; set; }
    }
}
