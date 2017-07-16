using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlannerPackages", "Description", c => c.String(nullable: false));
            DropColumn("dbo.EventPlannerPackages", "PackageCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventPlannerPackages", "PackageCategory", c => c.String(nullable: false));
            DropColumn("dbo.EventPlannerPackages", "Description");
        }
    }
}
