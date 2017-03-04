namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate24 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Quantity = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .Index(t => t.EventPlannerId);
            
            AddColumn("dbo.Prospects", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.Resources", new[] { "EventPlannerId" });
            DropColumn("dbo.Prospects", "Status");
            DropTable("dbo.Resources");
        }
    }
}
