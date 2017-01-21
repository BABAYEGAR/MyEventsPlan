namespace MyEventPlan.Data.DataContext.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class migration8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EventPlanners", "Firstname", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EventPlanners", "Firstname", c => c.String());
        }
    }
}
