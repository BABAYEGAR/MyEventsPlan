namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventPlanners", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.EventPlanners", "ConfirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventPlanners", "ConfirmPassword", c => c.String());
            AlterColumn("dbo.EventPlanners", "Password", c => c.String());
        }
    }
}
