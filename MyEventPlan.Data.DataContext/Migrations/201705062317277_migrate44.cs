using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate44 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPackages", "PackageGrade", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorPackages", "PackageGrade");
        }
    }
}
