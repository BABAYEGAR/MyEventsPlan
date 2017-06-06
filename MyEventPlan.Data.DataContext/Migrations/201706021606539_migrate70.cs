namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate70 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VendorServices", "Scale");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VendorServices", "Scale", c => c.String(nullable: false));
        }
    }
}
