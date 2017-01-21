namespace MyEventPlan.Data.DataContext.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class migration6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.AppUsers", new[] { "EventPlannerId" });
            AddColumn("dbo.EventPlanners", "Password", c => c.String());
            AddColumn("dbo.EventPlanners", "ConfirmPassword", c => c.String());
            AlterColumn("dbo.AppUsers", "EventPlannerId", c => c.Long());
            CreateIndex("dbo.AppUsers", "EventPlannerId");
            AddForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.AppUsers", new[] { "EventPlannerId" });
            AlterColumn("dbo.AppUsers", "EventPlannerId", c => c.Long(nullable: false));
            DropColumn("dbo.EventPlanners", "ConfirmPassword");
            DropColumn("dbo.EventPlanners", "Password");
            CreateIndex("dbo.AppUsers", "EventPlannerId");
            AddForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
        }
    }
}
