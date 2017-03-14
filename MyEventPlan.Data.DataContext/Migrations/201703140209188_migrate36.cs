namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Address = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        EventPlannerId = c.Long(),
                        EventId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clients", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Clients", "EventId", "dbo.Events");
            DropIndex("dbo.Clients", new[] { "EventId" });
            DropIndex("dbo.Clients", new[] { "EventPlannerId" });
            DropTable("dbo.Clients");
        }
    }
}
