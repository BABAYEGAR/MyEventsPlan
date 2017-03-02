namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewsActions",
                c => new
                    {
                        NewsActionId = c.Long(nullable: false, identity: true),
                        Action = c.String(),
                        NewsId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NewsActionId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: true)
                .Index(t => t.NewsId)
                .Index(t => t.AppUserId);
            
            AddColumn("dbo.News", "Likes", c => c.Long(nullable: false));
            AddColumn("dbo.News", "Dislike", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NewsActions", "NewsId", "dbo.News");
            DropForeignKey("dbo.NewsActions", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.NewsActions", new[] { "AppUserId" });
            DropIndex("dbo.NewsActions", new[] { "NewsId" });
            DropColumn("dbo.News", "Dislike");
            DropColumn("dbo.News", "Likes");
            DropTable("dbo.NewsActions");
        }
    }
}
