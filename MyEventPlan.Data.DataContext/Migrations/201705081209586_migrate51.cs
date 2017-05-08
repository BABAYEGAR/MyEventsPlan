namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
