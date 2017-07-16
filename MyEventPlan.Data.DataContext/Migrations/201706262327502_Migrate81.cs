using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate81 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointments", "EventId", "dbo.Events");
            DropIndex("dbo.Appointments", new[] { "EventId" });
            AddColumn("dbo.Appointments", "For", c => c.String(nullable: false));
            AlterColumn("dbo.Appointments", "EventId", c => c.Long());
            CreateIndex("dbo.Appointments", "EventId");
            AddForeignKey("dbo.Appointments", "EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "EventId", "dbo.Events");
            DropIndex("dbo.Appointments", new[] { "EventId" });
            AlterColumn("dbo.Appointments", "EventId", c => c.Long(nullable: false));
            DropColumn("dbo.Appointments", "For");
            CreateIndex("dbo.Appointments", "EventId");
            AddForeignKey("dbo.Appointments", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
    }
}
