namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
