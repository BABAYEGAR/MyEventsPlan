namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate21 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PersonalCheckListItems", "EventId", "dbo.Events");
            DropIndex("dbo.PersonalCheckListItems", new[] { "EventId" });
            DropColumn("dbo.PersonalCheckListItems", "EventId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PersonalCheckListItems", "EventId", c => c.Long(nullable: false));
            CreateIndex("dbo.PersonalCheckListItems", "EventId");
            AddForeignKey("dbo.PersonalCheckListItems", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
    }
}
