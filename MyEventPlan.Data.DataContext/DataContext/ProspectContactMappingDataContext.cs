using System.Data.Entity;
using Event.Data.Objects.Entities;

namespace MyEventPlan.Data.DataContext.DataContext
{
    public class ProspectContactMappingDataContext : DbContext
    {
        // Your context has been configured to use a 'ProspectContactMappingDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MyEventsPlan.Data.Context.DataContext.ProspectContactMappingDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ProspectContactMappingDataContext' 
        // connection string in the application configuration file.
        public ProspectContactMappingDataContext()
            : base("name=Event")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Event.Data.Objects.Entities.Event> Event { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<EventContactMapping> EventContactMapping { get; set; }
        public virtual DbSet<ProspectContactMapping> ProspectContactMapping { get; set; }
        public virtual DbSet<Prospect> Prospect { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
