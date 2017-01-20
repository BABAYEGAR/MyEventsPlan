namespace Event.Data.Objects.Entities
{
    public class Role : Transport
    {
        public long RoleId { get; set; }
        public string Name { get; set; }
        public bool ManageApplicationUser { get; set; }
        public bool ManageRoles { get; set; }
        public bool ManageEvents { get; set; }
        public bool ManageEventType { get; set; }
        public bool ManageVendors { get; set; }
        public bool ManageProspects { get; set; }
        public bool ManageInvoices { get; set; }
        public bool ManageContracts { get; set; }
        public bool ManageProposals { get; set; }
    }
}
