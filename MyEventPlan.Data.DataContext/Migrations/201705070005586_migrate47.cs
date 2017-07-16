using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate47 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", "dbo.EventPlannerPackages");
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerPackageId" });
            AlterColumn("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", c => c.Long());
            CreateIndex("dbo.EventPlannerPackageSettings", "EventPlannerPackageId");
            AddForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", "dbo.EventPlannerPackages", "EventPlannerPackageId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", "dbo.EventPlannerPackages");
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerPackageId" });
            AlterColumn("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", c => c.Long(nullable: false));
            CreateIndex("dbo.EventPlannerPackageSettings", "EventPlannerPackageId");
            AddForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", "dbo.EventPlannerPackages", "EventPlannerPackageId", cascadeDelete: true);
        }
    }
}
