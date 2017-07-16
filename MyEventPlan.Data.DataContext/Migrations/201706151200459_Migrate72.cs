using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate72 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventPlannerPackages", "Amount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventPlannerPackages", "Amount", c => c.String(nullable: false));
        }
    }
}
