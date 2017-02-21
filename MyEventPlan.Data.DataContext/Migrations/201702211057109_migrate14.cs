namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles");
            DropIndex("dbo.Contacts", new[] { "ContactRoleId" });
            AddColumn("dbo.Contacts", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Contacts", "EventPlannerId");
            AddForeignKey("dbo.Contacts", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            DropColumn("dbo.Contacts", "ContactRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "ContactRoleId", c => c.Long());
            DropForeignKey("dbo.Contacts", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Contacts", new[] { "EventPlannerId" });
            DropColumn("dbo.Contacts", "EventPlannerId");
            CreateIndex("dbo.Contacts", "ContactRoleId");
            AddForeignKey("dbo.Contacts", "ContactRoleId", "dbo.ContactRoles", "ContactRoleId");
        }
    }
}
