namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Name", c => c.String());
            AddColumn("dbo.Appointments", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "StartTime", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "EndTime", c => c.String(nullable: false));
            AddColumn("dbo.Appointments", "Location", c => c.String());
            AddColumn("dbo.Appointments", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Notes");
            DropColumn("dbo.Appointments", "Location");
            DropColumn("dbo.Appointments", "EndTime");
            DropColumn("dbo.Appointments", "EndDate");
            DropColumn("dbo.Appointments", "StartTime");
            DropColumn("dbo.Appointments", "StartDate");
            DropColumn("dbo.Appointments", "Name");
        }
    }
}
