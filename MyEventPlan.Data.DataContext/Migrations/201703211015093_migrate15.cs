namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "TargetBudget", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "TargetBudget", c => c.Long(nullable: false));
        }
    }
}