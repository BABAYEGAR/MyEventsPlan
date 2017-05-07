namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlannerPackages", "PackageGrade", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventPlannerPackages", "PackageGrade");
        }
    }
}
