namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mirate27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionInvoices",
                c => new
                    {
                        SubscriptionInvoiceId = c.Long(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        PackageId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.SubscriptionInvoiceId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.PackageId)
                .Index(t => t.AppUserId)
                .Index(t => t.EventPlannerId);
            
            AddColumn("dbo.EventPlannerPackages", "AppUserId", c => c.Long(nullable: false));
            CreateIndex("dbo.EventPlannerPackages", "AppUserId");
            AddForeignKey("dbo.EventPlannerPackages", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.SubscriptionInvoices", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.SubscriptionInvoices", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.EventPlannerPackages", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.SubscriptionInvoices", new[] { "EventPlannerId" });
            DropIndex("dbo.SubscriptionInvoices", new[] { "AppUserId" });
            DropIndex("dbo.SubscriptionInvoices", new[] { "PackageId" });
            DropIndex("dbo.EventPlannerPackages", new[] { "AppUserId" });
            DropColumn("dbo.EventPlannerPackages", "AppUserId");
            DropTable("dbo.SubscriptionInvoices");
        }
    }
}
