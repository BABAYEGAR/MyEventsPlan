namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate18 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Guests", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Guests", new[] { "EventPlannerId" });
            DropColumn("dbo.Guests", "EventPlannerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guests", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Guests", "EventPlannerId");
            AddForeignKey("dbo.Guests", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
    }
}
