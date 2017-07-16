using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prospects", "TargetBudget", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prospects", "TargetBudget", c => c.Long(nullable: false));
        }
    }
}
