namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "MinimumPrice", c => c.Long(nullable: false));
            AlterColumn("dbo.Vendors", "About", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "About", c => c.String(nullable: false));
            DropColumn("dbo.Vendors", "MinimumPrice");
        }
    }
}
