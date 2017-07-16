using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate58 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "Rating");
        }
    }
}
