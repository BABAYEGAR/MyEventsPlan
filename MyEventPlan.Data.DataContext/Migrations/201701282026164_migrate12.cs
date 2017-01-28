namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Appointments", "EventPlannerId");
            AddForeignKey("dbo.Appointments", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Appointments", new[] { "EventPlannerId" });
            DropColumn("dbo.Appointments", "EventPlannerId");
        }
    }
}
