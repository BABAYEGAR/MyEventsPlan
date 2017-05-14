namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate68 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "AverageRating", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventPlanners", "AverageRating");
        }
    }
}
