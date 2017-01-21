namespace MyEventPlan.Data.DataContext.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class migration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventPlannerId", c => c.Long());
            AddColumn("dbo.Prospects", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.Events", "EventPlannerId");
            CreateIndex("dbo.Prospects", "EventPlannerId");
            AddForeignKey("dbo.Events", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            AddForeignKey("dbo.Prospects", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prospects", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Events", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Prospects", new[] { "EventPlannerId" });
            DropIndex("dbo.Events", new[] { "EventPlannerId" });
            DropColumn("dbo.Prospects", "EventPlannerId");
            DropColumn("dbo.Events", "EventPlannerId");
        }
    }
}
