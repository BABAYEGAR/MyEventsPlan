namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate28 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "Image");
        }
    }
}
