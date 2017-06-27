using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Event.Data.Objects.Entities
{
    public class Appointment :Transport
    {
        public long AppointmentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string For { get; set; }
        public long? EventId { get; set; }
        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        [Required]
        [DisplayName("Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DisplayName("Start Time")]
        public string StartTime { get; set; }
        [Required]
        [DisplayName("End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [DisplayName("End Time")]
        public string EndTime { get; set; }
        [Required]
        public string Location { get; set; }
        public string Notes { get; set; }
        [DisplayName("Set Reminder?")]
        public bool SetReminder { get; set; }
        [DisplayName("Reminder Length")]
        public long? ReminderLength { get; set; }
        [DisplayName("Reminder Length Type")]
        public string ReminderLengthType { get; set; }
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
        public bool? SendEmailReminder { get; set; }
        public bool? SendTextMessageReminder { get; set; }
    }
}
