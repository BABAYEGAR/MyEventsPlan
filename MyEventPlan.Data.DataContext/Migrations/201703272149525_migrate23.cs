namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "BackgroundColor", c => c.String());
            DropColumn("dbo.AppUsers", "NavigationColor");
            DropColumn("dbo.AppUsers", "SideBarColor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "SideBarColor", c => c.String());
            AddColumn("dbo.AppUsers", "NavigationColor", c => c.String());
            DropColumn("dbo.AppUsers", "BackgroundColor");
        }
    }
}
