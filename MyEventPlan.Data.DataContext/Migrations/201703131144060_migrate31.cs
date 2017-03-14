namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vendors", "EventId", c => c.Long());
            CreateIndex("dbo.Vendors", "EventId");
            AddForeignKey("dbo.Vendors", "EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "EventId", "dbo.Events");
            DropIndex("dbo.Vendors", new[] { "EventId" });
            DropColumn("dbo.Vendors", "EventId");
        }
    }
}
