using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate71 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EventPlannerPackageItems", "Amount");
            DropColumn("dbo.VendorPackageItems", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VendorPackageItems", "Amount", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackageItems", "Amount", c => c.Long(nullable: false));
            AlterColumn("dbo.VendorPackages", "Amount", c => c.Long());
            AlterColumn("dbo.EventPlannerPackages", "Amount", c => c.Long());
        }
    }
}
