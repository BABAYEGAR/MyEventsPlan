using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate70 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.VendorServices", "Scale");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VendorServices", "Scale", c => c.String(nullable: false));
        }
    }
}
