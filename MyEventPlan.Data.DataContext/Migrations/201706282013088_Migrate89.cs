namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate89 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VisionBoards", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.VisionBoards", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VisionBoards", "Description", c => c.String());
            AlterColumn("dbo.VisionBoards", "Title", c => c.String());
        }
    }
}
