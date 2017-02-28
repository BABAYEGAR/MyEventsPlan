namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuestLists",
                c => new
                    {
                        GuestListId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EventId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.GuestListId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        EventId = c.Long(nullable: false),
                        GuestListId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.GuestId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.GuestLists", t => t.GuestListId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.GuestListId)
                .Index(t => t.EventPlannerId);
            
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
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventId)
                .Index(t => t.EventPlannerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Tasks", "EventId", "dbo.Events");
            DropForeignKey("dbo.Guests", "GuestListId", "dbo.GuestLists");
            DropForeignKey("dbo.Guests", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropForeignKey("dbo.GuestLists", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.GuestLists", "EventId", "dbo.Events");
            DropIndex("dbo.Tasks", new[] { "EventPlannerId" });
            DropIndex("dbo.Tasks", new[] { "EventId" });
            DropIndex("dbo.Guests", new[] { "EventPlannerId" });
            DropIndex("dbo.Guests", new[] { "GuestListId" });
            DropIndex("dbo.Guests", new[] { "EventId" });
            DropIndex("dbo.GuestLists", new[] { "EventPlannerId" });
            DropIndex("dbo.GuestLists", new[] { "EventId" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Guests");
            DropTable("dbo.GuestLists");
        }
    }
}
