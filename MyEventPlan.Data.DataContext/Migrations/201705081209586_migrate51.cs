using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate51 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Vendors", "PricingDetails");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "PricingDetails", c => c.Long(nullable: false));
        }
    }
}
