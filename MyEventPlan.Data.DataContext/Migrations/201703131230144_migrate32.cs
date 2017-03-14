namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "VendorId", c => c.Long());
            AddColumn("dbo.AppUsers", "Verified", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vendors", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Vendors", "ConfirmPassword", c => c.String(nullable: false));
            CreateIndex("dbo.AppUsers", "VendorId");
            AddForeignKey("dbo.AppUsers", "VendorId", "dbo.Vendors", "VendorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "VendorId", "dbo.Vendors");
            DropIndex("dbo.AppUsers", new[] { "VendorId" });
            DropColumn("dbo.Vendors", "ConfirmPassword");
            DropColumn("dbo.Vendors", "Password");
            DropColumn("dbo.AppUsers", "Verified");
            DropColumn("dbo.AppUsers", "VendorId");
        }
    }
}
