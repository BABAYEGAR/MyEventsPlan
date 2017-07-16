using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate90 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VisionBoards", "File", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VisionBoards", "File", c => c.String());
        }
    }
}
