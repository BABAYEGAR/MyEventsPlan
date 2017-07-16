namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate93 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GuestLists", "EventId", "dbo.Events");
            DropForeignKey("dbo.GuestLists", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.CustomQuestions", "GuestListId", "dbo.GuestLists");
            DropForeignKey("dbo.Guests", "GuestListId", "dbo.GuestLists");
            DropForeignKey("dbo.MealChoices", "GuestListId", "dbo.GuestLists");
            DropIndex("dbo.CustomQuestions", new[] { "GuestListId" });
            DropIndex("dbo.GuestLists", new[] { "EventId" });
            DropIndex("dbo.GuestLists", new[] { "EventPlannerId" });
            DropIndex("dbo.Guests", new[] { "GuestListId" });
            DropIndex("dbo.MealChoices", new[] { "GuestListId" });
            AddColumn("dbo.CustomQuestions", "GuestId", c => c.Long(nullable: true));
            AddColumn("dbo.MealChoices", "GuestId", c => c.Long(nullable: true));
            AlterColumn("dbo.ContactAddresses", "Type", c => c.String());
            AlterColumn("dbo.ContactAddresses", "Street1", c => c.String());
            AlterColumn("dbo.ContactAddresses", "PostalCode", c => c.String());
            AlterColumn("dbo.ContactAddresses", "State", c => c.String());
            AlterColumn("dbo.ContactAddresses", "Country", c => c.String());
            CreateIndex("dbo.CustomQuestions", "GuestId");
            CreateIndex("dbo.MealChoices", "GuestId");
            AddForeignKey("dbo.CustomQuestions", "GuestId", "dbo.Guests", "GuestId", cascadeDelete: false);
            AddForeignKey("dbo.MealChoices", "GuestId", "dbo.Guests", "GuestId", cascadeDelete: false);
            DropColumn("dbo.ContactAddresses", "Street2");
            DropColumn("dbo.CustomQuestions", "GuestListId");
            DropColumn("dbo.Guests", "GuestListId");
            DropColumn("dbo.MealChoices", "GuestListId");
            DropTable("dbo.GuestLists");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GuestLists",
                c => new
                    {
                        GuestListId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EventId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.GuestListId);
            
            AddColumn("dbo.MealChoices", "GuestListId", c => c.Long(nullable: false));
            AddColumn("dbo.Guests", "GuestListId", c => c.Long(nullable: false));
            AddColumn("dbo.CustomQuestions", "GuestListId", c => c.Long(nullable: false));
            AddColumn("dbo.ContactAddresses", "Street2", c => c.String(nullable: false));
            DropForeignKey("dbo.MealChoices", "GuestId", "dbo.Guests");
            DropForeignKey("dbo.CustomQuestions", "GuestId", "dbo.Guests");
            DropIndex("dbo.MealChoices", new[] { "GuestId" });
            DropIndex("dbo.CustomQuestions", new[] { "GuestId" });
            AlterColumn("dbo.ContactAddresses", "Country", c => c.String(nullable: false));
            AlterColumn("dbo.ContactAddresses", "State", c => c.String(nullable: false));
            AlterColumn("dbo.ContactAddresses", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.ContactAddresses", "Street1", c => c.String(nullable: false));
            AlterColumn("dbo.ContactAddresses", "Type", c => c.String(nullable: false));
            DropColumn("dbo.MealChoices", "GuestId");
            DropColumn("dbo.CustomQuestions", "GuestId");
            CreateIndex("dbo.MealChoices", "GuestListId");
            CreateIndex("dbo.Guests", "GuestListId");
            CreateIndex("dbo.GuestLists", "EventPlannerId");
            CreateIndex("dbo.GuestLists", "EventId");
            CreateIndex("dbo.CustomQuestions", "GuestListId");
            AddForeignKey("dbo.MealChoices", "GuestListId", "dbo.GuestLists", "GuestListId", cascadeDelete: true);
            AddForeignKey("dbo.Guests", "GuestListId", "dbo.GuestLists", "GuestListId", cascadeDelete: true);
            AddForeignKey("dbo.CustomQuestions", "GuestListId", "dbo.GuestLists", "GuestListId", cascadeDelete: true);
            AddForeignKey("dbo.GuestLists", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            AddForeignKey("dbo.GuestLists", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
    }
}
