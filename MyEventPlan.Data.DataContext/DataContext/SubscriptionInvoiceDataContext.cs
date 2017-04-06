﻿using System.Data.Entity;
using Event.Data.Objects.Entities;

namespace MyEventPlan.Data.DataContext.DataContext
{
    public class SubscriptionInvoiceDataContext : DbContext
    {
        // Your context has been configured to use a 'ContactDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MyEventsPlan.Data.Context.DataContext.ContactDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ContactDataContext' 
        // connection string in the application configuration file.
        public SubscriptionInvoiceDataContext()
            : base("name=Event")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Event.Data.Objects.Entities.Event> Event { get; set; }
        public virtual DbSet<Package> Packages { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<SubscriptionInvoice> SubscriptionInvoices { get; set; }
        public virtual DbSet<EventPlanner> EventPlanner { get; set; }
        public virtual DbSet<EventPlannerPackage> EventPlannerPackages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
