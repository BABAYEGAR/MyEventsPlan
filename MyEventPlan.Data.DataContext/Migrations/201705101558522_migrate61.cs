namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate61 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "AverageRating", c => c.Long());
            DropColumn("dbo.Vendors", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "Rating", c => c.Int(nullable: false));
            DropColumn("dbo.Vendors", "AverageRating");
        }
    }
}
