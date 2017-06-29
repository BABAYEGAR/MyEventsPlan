namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate91 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CustomQuestions", "Question", c => c.String(nullable: false));
            AlterColumn("dbo.CustomQuestions", "Answer", c => c.String(nullable: false));
            AlterColumn("dbo.MealChoices", "Choice", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MealChoices", "Choice", c => c.String());
            AlterColumn("dbo.CustomQuestions", "Answer", c => c.String());
            AlterColumn("dbo.CustomQuestions", "Question", c => c.String());
        }
    }
}
