using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate52 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "PricingDetails", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "PricingDetails");
        }
    }
}
