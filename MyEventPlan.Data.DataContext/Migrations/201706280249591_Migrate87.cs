namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate87 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomQuestions",
                c => new
                    {
                        CustomQuestionId = c.Long(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        GuestListId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.CustomQuestionId)
                .ForeignKey("dbo.GuestLists", t => t.GuestListId, cascadeDelete: true)
                .Index(t => t.GuestListId);
            
            CreateTable(
                "dbo.MealChoices",
                c => new
                    {
                        MealChoiceId = c.Long(nullable: false, identity: true),
                        Choice = c.String(),
                        GuestListId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.MealChoiceId)
                .ForeignKey("dbo.GuestLists", t => t.GuestListId, cascadeDelete: true)
                .Index(t => t.GuestListId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MealChoices", "GuestListId", "dbo.GuestLists");
            DropForeignKey("dbo.CustomQuestions", "GuestListId", "dbo.GuestLists");
            DropIndex("dbo.MealChoices", new[] { "GuestListId" });
            DropIndex("dbo.CustomQuestions", new[] { "GuestListId" });
            DropTable("dbo.MealChoices");
            DropTable("dbo.CustomQuestions");
        }
    }
}
