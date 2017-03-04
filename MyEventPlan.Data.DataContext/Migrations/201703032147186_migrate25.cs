namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventResourceMappings",
                c => new
                    {
                        EventResourceMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        ResourceId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventResourceMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Resources", t => t.ResourceId)
                .Index(t => t.EventId)
                .Index(t => t.ResourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.EventResourceMappings", "EventId", "dbo.Events");
            DropIndex("dbo.EventResourceMappings", new[] { "ResourceId" });
            DropIndex("dbo.EventResourceMappings", new[] { "EventId" });
            DropTable("dbo.EventResourceMappings");
        }
    }
}
