using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate45 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VendorPackages", "Amount", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VendorPackages", "Amount", c => c.Long(nullable: false));
        }
    }
}
