using System.Data.Entity;
using Event.Data.Objects.Entities;

namespace MyEventPlan.Data.DataContext.DataContext
{
    public class EventDataContext : DbContext
    {
        // Your context has been configured to use a 'EventDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MyEventsPlan.Data.Context.DataContext.EventsDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EventDataContext' 
        // connection string in the application configuration file.
        public EventDataContext()
            : base("name=Event")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Event.Data.Objects.Entities.Event> Event { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Prospect> Prospects { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactRole> ContactsRoles { get; set; }
        public virtual DbSet<EventContactMapping> EventContactMappings { get; set; }
        public virtual DbSet<EventVendorMapping> EventVendorMappings { get; set; }
        public virtual DbSet<StaffEventMapping> StaffEventMapping { get; set; }
        public virtual DbSet<ProspectContactMapping> ProspectContactMappings { get; set; }
        public virtual DbSet<EventPlanner> EventPlanners { get; set; }
        public virtual DbSet<VendorService> VendorServices { get; set; }
        public virtual DbSet<GuestList> GuestLists { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<CheckList> CheckLists { get; set; }
        public virtual DbSet<CheckListItem> CheckListItems { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<News> Newses { get; set; }
        public virtual DbSet<NewsAction> NewsActions { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<EventResourceMapping> EventResourceMapping { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageGroup> MessageGroups { get; set; }
        public virtual DbSet<MessageGroupMember> MessageGroupMembers { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PasswordReset> PasswordResets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
