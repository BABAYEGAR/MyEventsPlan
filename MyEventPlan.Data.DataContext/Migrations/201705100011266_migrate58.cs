namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate58 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "Rating");
        }
    }
}
