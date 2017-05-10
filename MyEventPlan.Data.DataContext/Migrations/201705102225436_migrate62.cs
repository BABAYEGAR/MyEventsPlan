namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate62 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invoices", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Invoices", new[] { "EventPlannerId" });
            AlterColumn("dbo.Invoices", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Invoices", "EventPlannerId");
            AddForeignKey("dbo.Invoices", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Invoices", new[] { "EventPlannerId" });
            AlterColumn("dbo.Invoices", "EventPlannerId", c => c.Long(nullable: false));
            CreateIndex("dbo.Invoices", "EventPlannerId");
            AddForeignKey("dbo.Invoices", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
        }
    }
}
