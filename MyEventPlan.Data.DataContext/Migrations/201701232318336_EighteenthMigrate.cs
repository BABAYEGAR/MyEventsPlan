namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EighteenthMigrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Status", c => c.String());
            DropColumn("dbo.Events", "StatusColor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "StatusColor", c => c.String());
            DropColumn("dbo.Events", "Status");
        }
    }
}
