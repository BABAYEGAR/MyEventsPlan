using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "LocationId", c => c.Long());
            CreateIndex("dbo.EventPlanners", "LocationId");
            AddForeignKey("dbo.EventPlanners", "LocationId", "dbo.Locations", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlanners", "LocationId", "dbo.Locations");
            DropIndex("dbo.EventPlanners", new[] { "LocationId" });
            DropColumn("dbo.EventPlanners", "LocationId");
        }
    }
}
