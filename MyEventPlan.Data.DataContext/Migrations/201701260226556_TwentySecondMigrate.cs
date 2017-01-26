namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentySecondMigrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Vendors", "EventPlannerId");
            AddForeignKey("dbo.Vendors", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Vendors", new[] { "EventPlannerId" });
            DropColumn("dbo.Vendors", "EventPlannerId");
        }
    }
}
