namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate17 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "EventId", "dbo.Events");
            DropForeignKey("dbo.Tasks", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Tasks", new[] { "EventId" });
            DropIndex("dbo.Tasks", new[] { "EventPlannerId" });
            CreateTable(
                "dbo.CheckListItems",
                c => new
                    {
                        CheckListItemId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Checked = c.Boolean(nullable: false),
                        EventId = c.Long(nullable: false),
                        CheckListId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.CheckListItemId)
                .ForeignKey("dbo.CheckLists", t => t.CheckListId, cascadeDelete: false)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.CheckListId);
            
            CreateTable(
                "dbo.CheckLists",
                c => new
                    {
                        CheckListId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.CheckListId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
            DropTable("dbo.Tasks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EventId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.TaskId);
            
            DropForeignKey("dbo.CheckListItems", "EventId", "dbo.Events");
            DropForeignKey("dbo.CheckListItems", "CheckListId", "dbo.CheckLists");
            DropForeignKey("dbo.CheckLists", "EventId", "dbo.Events");
            DropIndex("dbo.CheckLists", new[] { "EventId" });
            DropIndex("dbo.CheckListItems", new[] { "CheckListId" });
            DropIndex("dbo.CheckListItems", new[] { "EventId" });
            DropTable("dbo.CheckLists");
            DropTable("dbo.CheckListItems");
            CreateIndex("dbo.Tasks", "EventPlannerId");
            CreateIndex("dbo.Tasks", "EventId");
            AddForeignKey("dbo.Tasks", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            AddForeignKey("dbo.Tasks", "EventId", "dbo.Events", "EventId", cascadeDelete: true);
        }
    }
}
