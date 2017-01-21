namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles");
            DropForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects");
            DropIndex("dbo.Contacts", new[] { "ContactRoleId" });
            DropIndex("dbo.EventContactMappings", new[] { "EventId" });
            DropIndex("dbo.EventContactMappings", new[] { "ContactId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ProspectId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ContactId" });
            AlterColumn("dbo.Contacts", "ContactRoleId", c => c.Long());
            AlterColumn("dbo.EventContactMappings", "EventId", c => c.Long());
            AlterColumn("dbo.EventContactMappings", "ContactId", c => c.Long());
            AlterColumn("dbo.ProspectContactMappings", "ProspectId", c => c.Long());
            AlterColumn("dbo.ProspectContactMappings", "ContactId", c => c.Long());
            CreateIndex("dbo.Contacts", "ContactRoleId");
            CreateIndex("dbo.EventContactMappings", "EventId");
            CreateIndex("dbo.EventContactMappings", "ContactId");
            CreateIndex("dbo.ProspectContactMappings", "ProspectId");
            CreateIndex("dbo.ProspectContactMappings", "ContactId");
            AddForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles", "ContactRoleId");
            AddForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts", "ContactId");
            AddForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts", "ContactId");
            AddForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects", "ProspectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects");
            DropForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles");
            DropIndex("dbo.ProspectContactMappings", new[] { "ContactId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ProspectId" });
            DropIndex("dbo.EventContactMappings", new[] { "ContactId" });
            DropIndex("dbo.EventContactMappings", new[] { "EventId" });
            DropIndex("dbo.Contacts", new[] { "ContactRoleId" });
            AlterColumn("dbo.ProspectContactMappings", "ContactId", c => c.Long(nullable: false));
            AlterColumn("dbo.ProspectContactMappings", "ProspectId", c => c.Long(nullable: false));
            AlterColumn("dbo.EventContactMappings", "ContactId", c => c.Long(nullable: false));
            AlterColumn("dbo.EventContactMappings", "EventId", c => c.Long(nullable: false));
            AlterColumn("dbo.Contacts", "ContactRoleId", c => c.Long(nullable: false));
            CreateIndex("dbo.ProspectContactMappings", "ContactId");
            CreateIndex("dbo.ProspectContactMappings", "ProspectId");
            CreateIndex("dbo.EventContactMappings", "ContactId");
            CreateIndex("dbo.EventContactMappings", "EventId");
            CreateIndex("dbo.Contacts", "ContactRoleId");
            AddForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects", "ProspectId", cascadeDelete: true);
            AddForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts", "ContactId", cascadeDelete: true);
            AddForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
            AddForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts", "ContactId", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles", "ContactRoleId", cascadeDelete: true);
        }
    }
}
