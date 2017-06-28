namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate85 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoes", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.ToDoes", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ToDoes", "EventId", "dbo.Events");
            DropIndex("dbo.ToDoes", new[] { "EventId" });
            DropIndex("dbo.ToDoes", new[] { "ContactId" });
            DropIndex("dbo.ToDoes", new[] { "AppUserId" });
            AddColumn("dbo.ToDoes", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.ToDoes", "EventPlannerId", c => c.Long());
            AddColumn("dbo.ToDoes", "Notes", c => c.String());
            AlterColumn("dbo.ToDoes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ToDoes", "EventId", c => c.Long(nullable: false));
            AlterColumn("dbo.ToDoes", "ContactId", c => c.Long(nullable: false));
            AlterColumn("dbo.ToDoes", "AppUserId", c => c.Long(nullable: false));
            CreateIndex("dbo.ToDoes", "EventId");
            CreateIndex("dbo.ToDoes", "ContactId");
            CreateIndex("dbo.ToDoes", "AppUserId");
            CreateIndex("dbo.ToDoes", "EventPlannerId");
            AddForeignKey("dbo.ToDoes", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            AddForeignKey("dbo.ToDoes", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
            AddForeignKey("dbo.ToDoes", "ContactId", "dbo.Contacts", "ContactId", cascadeDelete: true);
            AddForeignKey("dbo.ToDoes", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoes", "EventId", "dbo.Events");
            DropForeignKey("dbo.ToDoes", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ToDoes", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.ToDoes", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.ToDoes", new[] { "EventPlannerId" });
            DropIndex("dbo.ToDoes", new[] { "AppUserId" });
            DropIndex("dbo.ToDoes", new[] { "ContactId" });
            DropIndex("dbo.ToDoes", new[] { "EventId" });
            AlterColumn("dbo.ToDoes", "AppUserId", c => c.Long());
            AlterColumn("dbo.ToDoes", "ContactId", c => c.Long());
            AlterColumn("dbo.ToDoes", "EventId", c => c.Long());
            AlterColumn("dbo.ToDoes", "Name", c => c.String());
            DropColumn("dbo.ToDoes", "Notes");
            DropColumn("dbo.ToDoes", "EventPlannerId");
            DropColumn("dbo.ToDoes", "DueDate");
            CreateIndex("dbo.ToDoes", "AppUserId");
            CreateIndex("dbo.ToDoes", "ContactId");
            CreateIndex("dbo.ToDoes", "EventId");
            AddForeignKey("dbo.ToDoes", "EventId", "dbo.Events", "EventId");
            AddForeignKey("dbo.ToDoes", "ContactId", "dbo.Contacts", "ContactId");
            AddForeignKey("dbo.ToDoes", "AppUserId", "dbo.AppUsers", "AppUserId");
        }
    }
}
