namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AppUserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Messages", "AppUserId");
            AddForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
            DropColumn("dbo.Messages", "Receipient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "Receipient", c => c.Long());
            DropForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.Messages", new[] { "AppUserId" });
            DropColumn("dbo.Messages", "AppUserId");
        }
    }
}
