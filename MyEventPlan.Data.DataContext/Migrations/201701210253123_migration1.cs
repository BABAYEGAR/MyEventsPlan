namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        EventTypeId = c.Long(nullable: false),
                        TargetBudget = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EndTime = c.String(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId, cascadeDelete: false)
                .Index(t => t.EventTypeId);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        EventTypeId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventTypeId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        AppUserId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        RoleId = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        ProfileImage = c.String(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.AppUserId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.RoleId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.EventPlanners",
                c => new
                    {
                        EventPlannerId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.EventPlannerId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManageApplicationUser = c.Boolean(nullable: false),
                        ManageRoles = c.Boolean(nullable: false),
                        ManageEvents = c.Boolean(nullable: false),
                        ManageEventType = c.Boolean(nullable: false),
                        ManageEventPlanners = c.Boolean(nullable: false),
                        ManageVendors = c.Boolean(nullable: false),
                        ManageVendorServices = c.Boolean(nullable: false),
                        ManageProspects = c.Boolean(nullable: false),
                        ManageInvoices = c.Boolean(nullable: false),
                        ManageContracts = c.Boolean(nullable: false),
                        ManageProposals = c.Boolean(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Firstname = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        ContactRoleId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.ContactRoles", t => t.ContactRoleId, cascadeDelete: false)
                .Index(t => t.ContactRoleId);
            
            CreateTable(
                "dbo.ContactRoles",
                c => new
                    {
                        ContactRoleId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RoleType = c.String(),
                        Employee = c.Boolean(nullable: false),
                        Client = c.Boolean(nullable: false),
                        Administrator = c.Boolean(nullable: false),
                        MasterAdministrator = c.Boolean(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ContactRoleId);
            
            CreateTable(
                "dbo.EventContactMappings",
                c => new
                    {
                        EventContactMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(nullable: false),
                        ContactId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.EventContactMappingId)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: false)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.ProspectContactMappings",
                c => new
                    {
                        ProspectContactMappingId = c.Long(nullable: false, identity: true),
                        ProspectId = c.Long(nullable: false),
                        ContactId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ProspectContactMappingId)
                .ForeignKey("dbo.Contacts", t => t.ContactId, cascadeDelete: false)
                .ForeignKey("dbo.Prospects", t => t.ProspectId, cascadeDelete: false)
                .Index(t => t.ProspectId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Prospects",
                c => new
                    {
                        ProspectId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Color = c.String(nullable: false),
                        EventTypeId = c.Long(nullable: false),
                        TargetBudget = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EndTime = c.String(nullable: false),
                        ContactRoleId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ProspectId)
                .ForeignKey("dbo.ContactRoles", t => t.ContactRoleId, cascadeDelete: false)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId, cascadeDelete: false)
                .Index(t => t.EventTypeId)
                .Index(t => t.ContactRoleId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        VendorServiceId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorId)
                .ForeignKey("dbo.VendorServices", t => t.VendorServiceId, cascadeDelete: false)
                .Index(t => t.VendorServiceId);
            
            CreateTable(
                "dbo.VendorServices",
                c => new
                    {
                        VendorServiceId = c.Long(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false),
                        Scale = c.String(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "VendorServiceId", "dbo.VendorServices");
            DropForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects");
            DropForeignKey("dbo.Prospects", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.Prospects", "ContactRoleId", "dbo.ContactRoles");
            DropForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles");
            DropForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Appointments", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "EventTypeId", "dbo.EventTypes");
            DropIndex("dbo.Vendors", new[] { "VendorServiceId" });
            DropIndex("dbo.Prospects", new[] { "ContactRoleId" });
            DropIndex("dbo.Prospects", new[] { "EventTypeId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ContactId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ProspectId" });
            DropIndex("dbo.EventContactMappings", new[] { "ContactId" });
            DropIndex("dbo.EventContactMappings", new[] { "EventId" });
            DropIndex("dbo.Contacts", new[] { "ContactRoleId" });
            DropIndex("dbo.EventPlanners", new[] { "RoleId" });
            DropIndex("dbo.AppUsers", new[] { "EventPlannerId" });
            DropIndex("dbo.AppUsers", new[] { "RoleId" });
            DropIndex("dbo.Events", new[] { "EventTypeId" });
            DropIndex("dbo.Appointments", new[] { "EventId" });
            DropTable("dbo.VendorServices");
            DropTable("dbo.Vendors");
            DropTable("dbo.Prospects");
            DropTable("dbo.ProspectContactMappings");
            DropTable("dbo.EventContactMappings");
            DropTable("dbo.ContactRoles");
            DropTable("dbo.Contacts");
            DropTable("dbo.Roles");
            DropTable("dbo.EventPlanners");
            DropTable("dbo.AppUsers");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Events");
            DropTable("dbo.Appointments");
        }
    }
}
