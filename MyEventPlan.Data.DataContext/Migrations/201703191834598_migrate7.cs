namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "Type", c => c.String());
            AddColumn("dbo.EventPlanners", "BusinessName", c => c.String());
            AddColumn("dbo.EventPlanners", "BusinessContact", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventPlanners", "BusinessContact");
            DropColumn("dbo.EventPlanners", "BusinessName");
            DropColumn("dbo.EventPlanners", "Type");
        }
    }
}
