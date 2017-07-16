using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPackageSettings", "Amount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorPackageSettings", "Amount");
        }
    }
}
