using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Prospects", "EventDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prospects", "EventDate");
            DropColumn("dbo.Events", "EventDate");
        }
    }
}
