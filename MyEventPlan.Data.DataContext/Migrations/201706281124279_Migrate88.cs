namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate88 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VisionBoardComments",
                c => new
                    {
                        VisionBoardCommentId = c.Long(nullable: false, identity: true),
                        Comment = c.String(),
                        VisionBoardId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VisionBoardCommentId)
                .ForeignKey("dbo.VisionBoards", t => t.VisionBoardId, cascadeDelete: true)
                .Index(t => t.VisionBoardId);
            
            CreateTable(
                "dbo.VisionBoards",
                c => new
                    {
                        VisionBoardId = c.Long(nullable: false, identity: true),
                        File = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VisionBoardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisionBoardComments", "VisionBoardId", "dbo.VisionBoards");
            DropIndex("dbo.VisionBoardComments", new[] { "VisionBoardId" });
            DropTable("dbo.VisionBoards");
            DropTable("dbo.VisionBoardComments");
        }
    }
}
