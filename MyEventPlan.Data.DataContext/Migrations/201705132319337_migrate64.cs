using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate64 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prospects", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Prospects", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Prospects", "Color", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prospects", "Color", c => c.String(nullable: false));
            DropColumn("dbo.Prospects", "PhoneNumber");
            DropColumn("dbo.Prospects", "Email");
        }
    }
}
