using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "StartTime", c => c.String());
            AlterColumn("dbo.Appointments", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "EndTime", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "StartTime", c => c.String(nullable: false));
        }
    }
}
