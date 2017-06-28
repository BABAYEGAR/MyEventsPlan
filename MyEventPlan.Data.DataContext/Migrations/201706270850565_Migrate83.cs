namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate83 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "SetReminder", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Appointments", "SendEmailReminder", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "SendEmailReminder", c => c.Boolean());
            AlterColumn("dbo.Appointments", "SetReminder", c => c.Boolean());
        }
    }
}
