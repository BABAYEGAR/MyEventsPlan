namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate46 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventPlannerPackageSettings", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerId" });
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "AppUserId" });
            AlterColumn("dbo.EventPlannerPackageSettings", "EventPlannerId", c => c.Long());
            AlterColumn("dbo.EventPlannerPackageSettings", "AppUserId", c => c.Long());
            CreateIndex("dbo.EventPlannerPackageSettings", "EventPlannerId");
            CreateIndex("dbo.EventPlannerPackageSettings", "AppUserId");
            AddForeignKey("dbo.EventPlannerPackageSettings", "AppUserId", "dbo.AppUsers", "AppUserId");
            AddForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlannerPackageSettings", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "AppUserId" });
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerId" });
            AlterColumn("dbo.EventPlannerPackageSettings", "AppUserId", c => c.Long(nullable: false));
            AlterColumn("dbo.EventPlannerPackageSettings", "EventPlannerId", c => c.Long(nullable: false));
            CreateIndex("dbo.EventPlannerPackageSettings", "AppUserId");
            CreateIndex("dbo.EventPlannerPackageSettings", "EventPlannerId");
            AddForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
            AddForeignKey("dbo.EventPlannerPackageSettings", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
        }
    }
}
