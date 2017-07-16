using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate84 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentContactMappings",
                c => new
                    {
                        AppointmentContactMappingId = c.Long(nullable: false, identity: true),
                        ContactId = c.Long(),
                        AppointmentId = c.Long()
                    })
                .PrimaryKey(t => t.AppointmentContactMappingId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.ContactAddresses",
                c => new
                    {
                        ContactAddressId = c.Long(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Street1 = c.String(nullable: false),
                        Street2 = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        ContactId = c.Long()
                    })
                .PrimaryKey(t => t.ContactAddressId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.ContactWebsites",
                c => new
                    {
                        ContactWebsiteId = c.Long(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Website = c.String(nullable: false),
                        ContactId = c.Long()
                    })
                .PrimaryKey(t => t.ContactWebsiteId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.ToDoes",
                c => new
                    {
                        ToDoId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EventId = c.Long(),
                        ContactId = c.Long(),
                        AppUserId = c.Long(),
                        SetReminder = c.Boolean(nullable: false)
                    })
                .PrimaryKey(t => t.ToDoId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId)
                .Index(t => t.ContactId)
                .Index(t => t.AppUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoes", "EventId", "dbo.Events");
            DropForeignKey("dbo.ToDoes", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ToDoes", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.ContactWebsites", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ContactAddresses", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.AppointmentContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.AppointmentContactMappings", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.ToDoes", new[] { "AppUserId" });
            DropIndex("dbo.ToDoes", new[] { "ContactId" });
            DropIndex("dbo.ToDoes", new[] { "EventId" });
            DropIndex("dbo.ContactWebsites", new[] { "ContactId" });
            DropIndex("dbo.ContactAddresses", new[] { "ContactId" });
            DropIndex("dbo.AppointmentContactMappings", new[] { "AppointmentId" });
            DropIndex("dbo.AppointmentContactMappings", new[] { "ContactId" });
            DropTable("dbo.ToDoes");
            DropTable("dbo.ContactWebsites");
            DropTable("dbo.ContactAddresses");
            DropTable("dbo.AppointmentContactMappings");
        }
    }
}
