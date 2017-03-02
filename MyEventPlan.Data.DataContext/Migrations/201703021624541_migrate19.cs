namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "EventId", "dbo.Events");
            DropIndex("dbo.Notes", new[] { "EventId" });
            DropTable("dbo.Notes");
        }
    }
}
