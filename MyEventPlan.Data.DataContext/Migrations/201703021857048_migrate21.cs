namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate21 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Status = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "EventId", "dbo.Events");
            DropIndex("dbo.Tasks", new[] { "EventId" });
            DropTable("dbo.Tasks");
        }
    }
}
