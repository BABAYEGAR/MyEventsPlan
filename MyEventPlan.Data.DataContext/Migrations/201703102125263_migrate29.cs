namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate29 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "StartTime", c => c.String());
            AlterColumn("dbo.Events", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EndTime", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "StartTime", c => c.String(nullable: false));
        }
    }
}
