namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate42 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPlannerPackageItems",
                c => new
                    {
                        EventPlannerPackageItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Amount = c.Long(nullable: false),
                        EventPlannerPackageId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventPlannerPackageItemId)
                .ForeignKey("dbo.EventPlannerPackages", t => t.EventPlannerPackageId, cascadeDelete: true)
                .Index(t => t.EventPlannerPackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlannerPackageItems", "EventPlannerPackageId", "dbo.EventPlannerPackages");
            DropIndex("dbo.EventPlannerPackageItems", new[] { "EventPlannerPackageId" });
            DropTable("dbo.EventPlannerPackageItems");
        }
    }
}
