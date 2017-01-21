namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles");
            DropIndex("dbo.EventPlanners", new[] { "RoleId" });
            DropIndex("dbo.AppUsers", new[] { "RoleId" });
            AlterColumn("dbo.EventPlanners", "RoleId", c => c.Long());
            AlterColumn("dbo.AppUsers", "RoleId", c => c.Long());
            CreateIndex("dbo.EventPlanners", "RoleId");
            CreateIndex("dbo.AppUsers", "RoleId");
            AddForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles", "RoleId");
            AddForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles", "RoleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles");
            DropIndex("dbo.AppUsers", new[] { "RoleId" });
            DropIndex("dbo.EventPlanners", new[] { "RoleId" });
            AlterColumn("dbo.AppUsers", "RoleId", c => c.Long(nullable: false));
            AlterColumn("dbo.EventPlanners", "RoleId", c => c.Long(nullable: false));
            CreateIndex("dbo.AppUsers", "RoleId");
            CreateIndex("dbo.EventPlanners", "RoleId");
            AddForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
            AddForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles", "RoleId", cascadeDelete: true);
        }
    }
}
