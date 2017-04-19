namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate32 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContactRoles", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects");
            DropIndex("dbo.ContactRoles", new[] { "EventPlannerId" });
            DropIndex("dbo.EventContactMappings", new[] { "EventId" });
            DropIndex("dbo.EventContactMappings", new[] { "ContactId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ProspectId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ContactId" });
            DropTable("dbo.ContactRoles");
            DropTable("dbo.EventContactMappings");
            DropTable("dbo.ProspectContactMappings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProspectContactMappings",
                c => new
                    {
                        ProspectContactMappingId = c.Long(nullable: false, identity: true),
                        ProspectId = c.Long(),
                        ContactId = c.Long(),
                    })
                .PrimaryKey(t => t.ProspectContactMappingId);
            
            CreateTable(
                "dbo.EventContactMappings",
                c => new
                    {
                        EventContactMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        ContactId = c.Long(),
                    })
                .PrimaryKey(t => t.EventContactMappingId);
            
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
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ContactRoleId);
            
            CreateIndex("dbo.ProspectContactMappings", "ContactId");
            CreateIndex("dbo.ProspectContactMappings", "ProspectId");
            CreateIndex("dbo.EventContactMappings", "ContactId");
            CreateIndex("dbo.EventContactMappings", "EventId");
            CreateIndex("dbo.ContactRoles", "EventPlannerId");
            AddForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects", "ProspectId");
            AddForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts", "ContactId");
            AddForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts", "ContactId");
            AddForeignKey("dbo.ContactRoles", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
    }
}
