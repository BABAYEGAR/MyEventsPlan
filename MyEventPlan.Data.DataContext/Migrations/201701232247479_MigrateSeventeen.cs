namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateSeventeen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "StatusColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "StatusColor");
        }
    }
}
