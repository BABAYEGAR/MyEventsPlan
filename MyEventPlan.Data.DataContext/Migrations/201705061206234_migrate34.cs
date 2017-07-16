using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate34 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventPlannerPackages", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.EventPlannerPackages", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlannerPackages", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.SubscriptionInvoices", "PackageId", "dbo.Packages");
            DropIndex("dbo.EventPlannerPackages", new[] { "PackageId" });
            DropIndex("dbo.EventPlannerPackages", new[] { "EventPlannerId" });
            DropIndex("dbo.EventPlannerPackages", new[] { "AppUserId" });
            DropIndex("dbo.SubscriptionInvoices", new[] { "PackageId" });
            CreateTable(
                "dbo.EventPlannerPackageSettings",
                c => new
                    {
                        EventPlannerPackageSettingId = c.Long(nullable: false, identity: true),
                        Status = c.String(),
                        SubscribedEvent = c.Long(nullable: false),
                        AllowedEvent = c.Long(nullable: false),
                        EventPlannerPackageId = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.EventPlannerPackageSettingId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .ForeignKey("dbo.EventPlannerPackages", t => t.EventPlannerPackageId, cascadeDelete: true)
                .Index(t => t.EventPlannerPackageId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.AppUserId);
            
            AddColumn("dbo.EventPlannerPackages", "PackageName", c => c.String(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "PackageCategory", c => c.String(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "Amount", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "MaximumEvents", c => c.Long(nullable: false));
            AddColumn("dbo.SubscriptionInvoices", "Package_EventPlannerPackageId", c => c.Long());
            CreateIndex("dbo.SubscriptionInvoices", "Package_EventPlannerPackageId");
            AddForeignKey("dbo.SubscriptionInvoices", "Package_EventPlannerPackageId", "dbo.EventPlannerPackages", "EventPlannerPackageId");
            DropColumn("dbo.EventPlannerPackages", "Status");
            DropColumn("dbo.EventPlannerPackages", "SubscribedEvent");
            DropColumn("dbo.EventPlannerPackages", "AllowedEvent");
            DropColumn("dbo.EventPlannerPackages", "PackageId");
            DropColumn("dbo.EventPlannerPackages", "EventPlannerId");
            DropColumn("dbo.EventPlannerPackages", "AppUserId");
            DropTable("dbo.Packages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageId = c.Long(nullable: false, identity: true),
                        PackageName = c.String(nullable: false),
                        Amount = c.Long(nullable: false),
                        MaximumEvents = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.PackageId);
            
            AddColumn("dbo.EventPlannerPackages", "AppUserId", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "EventPlannerId", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "PackageId", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "AllowedEvent", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "SubscribedEvent", c => c.Long(nullable: false));
            AddColumn("dbo.EventPlannerPackages", "Status", c => c.String());
            DropForeignKey("dbo.SubscriptionInvoices", "Package_EventPlannerPackageId", "dbo.EventPlannerPackages");
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerPackageId", "dbo.EventPlannerPackages");
            DropForeignKey("dbo.EventPlannerPackageSettings", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlannerPackageSettings", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.SubscriptionInvoices", new[] { "Package_EventPlannerPackageId" });
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "AppUserId" });
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerId" });
            DropIndex("dbo.EventPlannerPackageSettings", new[] { "EventPlannerPackageId" });
            DropColumn("dbo.SubscriptionInvoices", "Package_EventPlannerPackageId");
            DropColumn("dbo.EventPlannerPackages", "MaximumEvents");
            DropColumn("dbo.EventPlannerPackages", "Amount");
            DropColumn("dbo.EventPlannerPackages", "PackageCategory");
            DropColumn("dbo.EventPlannerPackages", "PackageName");
            DropTable("dbo.EventPlannerPackageSettings");
            CreateIndex("dbo.SubscriptionInvoices", "PackageId");
            CreateIndex("dbo.EventPlannerPackages", "AppUserId");
            CreateIndex("dbo.EventPlannerPackages", "EventPlannerId");
            CreateIndex("dbo.EventPlannerPackages", "PackageId");
            AddForeignKey("dbo.SubscriptionInvoices", "PackageId", "dbo.Packages", "PackageId", cascadeDelete: true);
            AddForeignKey("dbo.EventPlannerPackages", "PackageId", "dbo.Packages", "PackageId", cascadeDelete: true);
            AddForeignKey("dbo.EventPlannerPackages", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
            AddForeignKey("dbo.EventPlannerPackages", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
        }
    }
}
