using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "Name", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.EventPlanners", "Firstname");
            DropColumn("dbo.EventPlanners", "Lastname");
            DropColumn("dbo.EventPlanners", "BusinessName");
            DropColumn("dbo.EventPlanners", "BusinessContact");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EventPlanners", "BusinessContact", c => c.String());
            AddColumn("dbo.EventPlanners", "BusinessName", c => c.String());
            AddColumn("dbo.EventPlanners", "Lastname", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.EventPlanners", "Firstname", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.EventPlanners", "Name");
        }
    }
}
