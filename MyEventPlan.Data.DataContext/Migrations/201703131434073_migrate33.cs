namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "About", c => c.String(nullable: false));
            AddColumn("dbo.Vendors", "ImageOne", c => c.String());
            AddColumn("dbo.Vendors", "ImageTwo", c => c.String());
            AddColumn("dbo.Vendors", "ImageThree", c => c.String());
            DropColumn("dbo.Vendors", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "Image", c => c.String());
            DropColumn("dbo.Vendors", "ImageThree");
            DropColumn("dbo.Vendors", "ImageTwo");
            DropColumn("dbo.Vendors", "ImageOne");
            DropColumn("dbo.Vendors", "About");
        }
    }
}
