using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate69 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.SubscriptionInvoices", new[] { "AppUserId" });
            AlterColumn("dbo.SubscriptionInvoices", "AppUserId", c => c.Long());
            CreateIndex("dbo.SubscriptionInvoices", "AppUserId");
            AddForeignKey("dbo.SubscriptionInvoices", "AppUserId", "dbo.AppUsers", "AppUserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionInvoices", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.SubscriptionInvoices", new[] { "AppUserId" });
            AlterColumn("dbo.SubscriptionInvoices", "AppUserId", c => c.Long(nullable: false));
            CreateIndex("dbo.SubscriptionInvoices", "AppUserId");
            AddForeignKey("dbo.SubscriptionInvoices", "AppUserId", "dbo.AppUsers", "AppUserId", cascadeDelete: true);
        }
    }
}
