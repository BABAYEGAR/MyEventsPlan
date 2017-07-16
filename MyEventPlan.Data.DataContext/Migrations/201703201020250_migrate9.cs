using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventResourceMappings", "Quantity", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventResourceMappings", "Quantity");
        }
    }
}
