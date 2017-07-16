using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate60 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VendorReviews", "Rating", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VendorReviews", "Rating", c => c.Long(nullable: false));
        }
    }
}
