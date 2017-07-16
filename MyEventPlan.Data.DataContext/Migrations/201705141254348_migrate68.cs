using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate68 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "AverageRating", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventPlanners", "AverageRating");
        }
    }
}
