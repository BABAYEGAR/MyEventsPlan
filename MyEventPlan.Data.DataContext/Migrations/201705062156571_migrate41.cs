using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate41 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventPlannerPackages", "Amount", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventPlannerPackages", "Amount", c => c.Long(nullable: false));
        }
    }
}
