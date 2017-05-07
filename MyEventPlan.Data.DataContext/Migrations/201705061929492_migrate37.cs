namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPackageSettings", "Amount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorPackageSettings", "Amount");
        }
    }
}
