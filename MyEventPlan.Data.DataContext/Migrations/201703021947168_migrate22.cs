namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate22 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        NewsImage = c.String(),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .Index(t => t.EventPlannerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.News", new[] { "EventPlannerId" });
            DropTable("dbo.News");
        }
    }
}
