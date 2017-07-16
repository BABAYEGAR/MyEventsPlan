using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Event.Data.Objects.Entities
{
    public class Event : Transport
    {
        public long EventId { get; set; }
        [Required]
        [DisplayName("Event Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Event Color")]
        public string Color { get; set; }
        public string Status { get; set; }
        [Required]
        [DisplayName("Event Type")]
        public long? EventTypeId { get; set; }
        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }
        [Required]
        [DisplayName("Target Budget")]
        public string TargetBudget { get; set; }
        [Required]
        [DisplayName("Event Date")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
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
        [DisplayName("Event Planner")]
        public long? EventPlannerId { get; set; }
        [ForeignKey("EventPlannerId")]
        public virtual EventPlanner EventPlanner { get; set; }
        public long? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<EventVendorMapping> EventVendorMappings { get; set; }
        public IEnumerable<StaffEventMapping> StaffEventMapping { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<CheckList> CheckLists { get; set; }
        public IEnumerable<CheckListItem> CheckListItems { get; set; }
        public IEnumerable<Task> Tasks { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<News> Newses { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Budget> Budgets { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public IEnumerable<EventPlannerPackageSetting> EventPlannerPackages { get; set; }
    }
}
