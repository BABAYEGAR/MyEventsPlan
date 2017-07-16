using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate36 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPackages", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Vendors", "BusinessName");
            DropColumn("dbo.Vendors", "BusinessContact");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "BusinessContact", c => c.String(nullable: false));
            AddColumn("dbo.Vendors", "BusinessName", c => c.String(nullable: false));
            DropColumn("dbo.VendorPackages", "Description");
        }
    }
}
