using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "NavigationColor", c => c.String());
            AddColumn("dbo.AppUsers", "SideBarColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "SideBarColor");
            DropColumn("dbo.AppUsers", "NavigationColor");
        }
    }
}
