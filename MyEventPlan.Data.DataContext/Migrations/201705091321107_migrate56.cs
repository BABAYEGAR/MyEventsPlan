namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate56 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors");
            DropIndex("dbo.SubscriptionInvoices", new[] { "EventPlannerId" });
            DropIndex("dbo.SubscriptionInvoices", new[] { "VendorId" });
            AlterColumn("dbo.SubscriptionInvoices", "EventPlannerId", c => c.Long());
            AlterColumn("dbo.SubscriptionInvoices", "VendorId", c => c.Long());
            CreateIndex("dbo.SubscriptionInvoices", "EventPlannerId");
            CreateIndex("dbo.SubscriptionInvoices", "VendorId");
            AddForeignKey("dbo.SubscriptionInvoices", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId");
            AddForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors", "VendorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.SubscriptionInvoices", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.SubscriptionInvoices", new[] { "VendorId" });
            DropIndex("dbo.SubscriptionInvoices", new[] { "EventPlannerId" });
            AlterColumn("dbo.SubscriptionInvoices", "VendorId", c => c.Long(nullable: false));
            AlterColumn("dbo.SubscriptionInvoices", "EventPlannerId", c => c.Long(nullable: false));
            CreateIndex("dbo.SubscriptionInvoices", "VendorId");
            CreateIndex("dbo.SubscriptionInvoices", "EventPlannerId");
            AddForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors", "VendorId", cascadeDelete: true);
            AddForeignKey("dbo.SubscriptionInvoices", "EventPlannerId", "dbo.EventPlanners", "EventPlannerId", cascadeDelete: true);
        }
    }
}
