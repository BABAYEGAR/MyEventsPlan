using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("Manage Event Planners")]
        public bool ManageEventPlanners { get; set; }
        [DisplayName("Manage Vendors")]
        public bool ManageVendors { get; set; }
        [DisplayName("Manage Vendor Services")]
        public bool ManageVendorServices { get; set; }
        [DisplayName("Manage Prospects")]
        public bool ManageProspects { get; set; }
        [DisplayName("Manage Invoices")]
        public bool ManageInvoices { get; set; }
        [DisplayName("Manage Contracts")]
        public bool ManageContracts { get; set; }
        [DisplayName("Manage Proposals")]
        public bool ManageProposals { get; set; }
        public IEnumerable<EventPlanner> EventPlanners { get; set; }
        public IEnumerable<AppUser> AppUsers { get; set; }
    }
}
