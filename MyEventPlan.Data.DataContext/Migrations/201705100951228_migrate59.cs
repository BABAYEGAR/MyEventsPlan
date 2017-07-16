using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate59 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VendorReviews", "Rating", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VendorReviews", "Rating", c => c.String());
        }
    }
}
