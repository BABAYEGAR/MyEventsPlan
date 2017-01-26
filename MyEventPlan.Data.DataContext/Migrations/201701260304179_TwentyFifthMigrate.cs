namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentyFifthMigrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventVendorMappings", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.EventVendorMappings", "EventPlannerId");
            AddForeignKey("dbo.EventVendorMappings", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventVendorMappings", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.EventVendorMappings", new[] { "EventPlannerId" });
            DropColumn("dbo.EventVendorMappings", "EventPlannerId");
        }
    }
}
