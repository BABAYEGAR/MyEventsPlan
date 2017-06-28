namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate82 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "SetReminder", c => c.Boolean());
            AddColumn("dbo.Appointments", "ReminderLength", c => c.Long());
            AddColumn("dbo.Appointments", "ReminderLengthType", c => c.String());
            AddColumn("dbo.Appointments", "ReminderRepeat", c => c.String());
            AddColumn("dbo.Appointments", "ContactId", c => c.Long());
            AddColumn("dbo.Appointments", "SendEmailReminder", c => c.Boolean());
            AddColumn("dbo.Appointments", "SendTextMessageReminder", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "SendTextMessageReminder");
            DropColumn("dbo.Appointments", "SendEmailReminder");
            DropColumn("dbo.Appointments", "ContactId");
            DropColumn("dbo.Appointments", "ReminderRepeat");
            DropColumn("dbo.Appointments", "ReminderLengthType");
            DropColumn("dbo.Appointments", "ReminderLength");
            DropColumn("dbo.Appointments", "SetReminder");
        }
    }
}
