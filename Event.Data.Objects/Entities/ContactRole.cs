using System.Collections.Generic;

namespace Event.Data.Objects.Entities
{
    public class ContactRole :Transport
    {
        public long ContactRoleId { get; set; }
        public string Name { get; set; }
        public string RoleType { get; set; }
        public bool Employee { get; set; }
        public bool Client { get; set; }
        public bool Administrator { get; set; }
        public bool MasterAdministrator { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
