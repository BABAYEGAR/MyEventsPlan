namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate25 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "Location", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Location", c => c.String());
            AlterColumn("dbo.Appointments", "Name", c => c.String());
        }
    }
}
