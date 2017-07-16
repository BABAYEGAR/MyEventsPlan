using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate63 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Resources", new[] { "EventPlannerId" });
            AlterColumn("dbo.Resources", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Resources", "EventPlannerId");
            AddForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Resources", new[] { "EventPlannerId" });
            AlterColumn("dbo.Resources", "EventPlannerId", c => c.Long(nullable: false));
            CreateIndex("dbo.Resources", "EventPlannerId");
            AddForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
        }
    }
}
