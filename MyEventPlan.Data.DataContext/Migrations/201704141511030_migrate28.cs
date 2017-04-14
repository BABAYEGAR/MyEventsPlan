namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate28 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Budgets", "AmountStillDue", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Budgets", "AmountStillDue", c => c.Long(nullable: false));
        }
    }
}
