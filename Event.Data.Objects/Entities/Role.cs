using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Role : Transport
    {
        public long RoleId { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Manage Application User")]
        public bool ManageApplicationUser { get; set; }
        [DisplayName("Manage Roles")]
        public bool ManageRoles { get; set; }
        [DisplayName("Manage Events")]
        public bool ManageEvents { get; set; }
        [DisplayName("Manage Event Types")]
        public bool ManageEventType { get; set; }
        [DisplayName("Manage Vendors")]
        public bool ManageVendors { get; set; }
        [DisplayName("Manage Event Vendors")]
        public bool ManageEventVendors { get; set; }
        [DisplayName("Manage Vendor Services")]
        public bool ManageVendorServices { get; set; }
        [DisplayName("Manage Calendar")]
        public bool ManageCalendar { get; set; }
        [DisplayName("Manage Staff")]
        public bool ManageStaff { get; set; }
        [DisplayName("Manage Resources")]
        public bool ManageResources { get; set; }
        [DisplayName("Manage Prospects")]
        public bool ManageProspects { get; set; }
        [DisplayName("Manage Invoices")]
        public bool ManageInvoices { get; set; }
        [DisplayName("Manage GuestList")]
        public bool ManageGuestList { get; set; }
        [DisplayName("Manage CheckList")]
        public bool ManageCheckList { get; set; }
        [DisplayName("Manage Budgets")]
        public bool ManageBudgets { get; set; }
        [DisplayName("Manage Contacts")]
        public bool ManageContacts { get; set; }
        [DisplayName("Manage Pacakges")]
        public bool ManagePackages { get; set; }
        [DisplayName("Manage Notes")]
        public bool ManageNotes { get; set; }
        [DisplayName("Manage Locations")]
        public bool ManageLocations { get; set; }
        [DisplayName("Manage Task")]
        public bool ManageTasks { get; set; }
        public IEnumerable<Staff> Staff { get; set; }
        public IEnumerable<EventPlanner> EventPlanners { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
    }
}
