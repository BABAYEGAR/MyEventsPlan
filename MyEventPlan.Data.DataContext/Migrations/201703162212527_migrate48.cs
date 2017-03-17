namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate48 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prospects", "StartTime", c => c.String());
            AlterColumn("dbo.Prospects", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prospects", "EndTime", c => c.String(nullable: false));
            AlterColumn("dbo.Prospects", "StartTime", c => c.String(nullable: false));
        }
    }
}
