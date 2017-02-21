namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "Status", c => c.String());
            AddColumn("dbo.ContactRoles", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.ContactRoles", "EventPlannerId");
            AddForeignKey("dbo.ContactRoles", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactRoles", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.ContactRoles", new[] { "EventPlannerId" });
            DropColumn("dbo.ContactRoles", "EventPlannerId");
            DropColumn("dbo.AppUsers", "Status");
        }
    }
}
