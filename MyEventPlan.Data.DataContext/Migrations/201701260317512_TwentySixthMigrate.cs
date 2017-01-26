namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentySixthMigrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventVendorMappings", "CreatedBy", c => c.Long());
            AddColumn("dbo.EventVendorMappings", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventVendorMappings", "DateLastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.EventVendorMappings", "LastModifiedBy", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventVendorMappings", "LastModifiedBy");
            DropColumn("dbo.EventVendorMappings", "DateLastModified");
            DropColumn("dbo.EventVendorMappings", "DateCreated");
            DropColumn("dbo.EventVendorMappings", "CreatedBy");
        }
    }
}
