namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate55 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionInvoices", "VendorId", c => c.Long(nullable: true));
            CreateIndex("dbo.SubscriptionInvoices", "VendorId");
            AddForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors", "VendorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "VendorId", "dbo.Vendors");
            DropIndex("dbo.SubscriptionInvoices", new[] { "VendorId" });
            DropColumn("dbo.SubscriptionInvoices", "VendorId");
        }
    }
}
