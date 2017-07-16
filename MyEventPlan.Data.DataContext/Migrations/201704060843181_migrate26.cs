using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPlannerPackages",
                c => new
                    {
                        EventPlannerPackageId = c.Long(nullable: false, identity: true),
                        Status = c.String(),
                        SubscribedEvent = c.Long(nullable: false),
                        AllowedEvent = c.Long(nullable: false),
                        PackageId = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.EventPlannerPackageId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.PackageId)
                .Index(t => t.EventPlannerId);
            
            AlterColumn("dbo.Packages", "Amount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlannerPackages", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.EventPlannerPackages", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.EventPlannerPackages", new[] { "EventPlannerId" });
            DropIndex("dbo.EventPlannerPackages", new[] { "PackageId" });
            AlterColumn("dbo.Packages", "Amount", c => c.String());
            DropTable("dbo.EventPlannerPackages");
        }
    }
}
