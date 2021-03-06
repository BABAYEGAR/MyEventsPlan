﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Contact :Transport
    {
        public long ContactId { get; set; }
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
        [DisplayName("Event Planner")]
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public IEnumerable<AppointmentContactMapping> AppointmentContactMappings { get; set; }
        public IEnumerable<ContactAddress> ContactAddresses { get; set; }
        public IEnumerable<ToDo> ToDoS { get; set; }
        public IEnumerable<ContactWebsite> Websites { get; set; }
        public IEnumerable<Prospect> Prospects { get; set; }
        public string DisplayName
=> Firstname + " " + Lastname;
    }
}
