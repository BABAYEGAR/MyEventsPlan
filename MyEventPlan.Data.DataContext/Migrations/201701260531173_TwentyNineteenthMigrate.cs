namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentyNineteenthMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StaffEventMappings",
                c => new
                    {
                        StaffEventMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        StaffId = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.StaffEventMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Staffs", t => t.StaffId)
                .Index(t => t.EventId)
                .Index(t => t.StaffId)
                .Index(t => t.EventPlannerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.StaffEventMappings", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.StaffEventMappings", "EventId", "dbo.Events");
            DropIndex("dbo.StaffEventMappings", new[] { "EventPlannerId" });
            DropIndex("dbo.StaffEventMappings", new[] { "StaffId" });
            DropIndex("dbo.StaffEventMappings", new[] { "EventId" });
            DropTable("dbo.StaffEventMappings");
        }
    }
}
