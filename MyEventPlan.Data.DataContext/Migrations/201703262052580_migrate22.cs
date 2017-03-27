namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate22 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.Messages", new[] { "AppUserId" });
            AlterColumn("dbo.Messages", "AppUserId", c => c.Long());
            CreateIndex("dbo.Messages", "AppUserId");
            AddForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers", "AppUserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.Messages", new[] { "AppUserId" });
            AlterColumn("dbo.Messages", "AppUserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Messages", "AppUserId");
            AddForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
        }
    }
}
