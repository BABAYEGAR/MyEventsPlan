namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate57 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "MinimumPrice", c => c.Long());
            AddColumn("dbo.Vendors", "MaximumPrice", c => c.Long());
            DropColumn("dbo.Vendors", "AveragePrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "AveragePrice", c => c.Long(nullable: false));
            DropColumn("dbo.Vendors", "MaximumPrice");
            DropColumn("dbo.Vendors", "MinimumPrice");
        }
    }
}
