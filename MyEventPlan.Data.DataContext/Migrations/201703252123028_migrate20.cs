namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
