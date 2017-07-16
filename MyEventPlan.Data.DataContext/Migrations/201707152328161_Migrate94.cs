namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate94 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetPayments",
                c => new
                    {
                        BudgetPaymentId = c.Long(nullable: false, identity: true),
                        AmountPaid = c.Long(nullable: false),
                        DatePaid = c.DateTime(nullable: false),
                        BudgetId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetPaymentId)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .Index(t => t.BudgetId);
            
            AddColumn("dbo.Clients", "Title", c => c.String(nullable: false));
            AddColumn("dbo.Budgets", "VendorId", c => c.Long());
            AlterColumn("dbo.Budgets", "EstimatedAmount", c => c.Long());
            AlterColumn("dbo.Budgets", "NegotiatedAmount", c => c.Long());
            AlterColumn("dbo.Budgets", "ActualAmount", c => c.Long());
            AlterColumn("dbo.Budgets", "PaidTillDate", c => c.Long());
            CreateIndex("dbo.Budgets", "VendorId");
            AddForeignKey("dbo.Budgets", "VendorId", "dbo.Vendors", "VendorId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetPayments", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Budgets", "VendorId", "dbo.Vendors");
            DropIndex("dbo.Budgets", new[] { "VendorId" });
            DropIndex("dbo.BudgetPayments", new[] { "BudgetId" });
            AlterColumn("dbo.Budgets", "PaidTillDate", c => c.Long(nullable: false));
            AlterColumn("dbo.Budgets", "ActualAmount", c => c.Long(nullable: false));
            AlterColumn("dbo.Budgets", "NegotiatedAmount", c => c.Long(nullable: false));
            AlterColumn("dbo.Budgets", "EstimatedAmount", c => c.Long(nullable: false));
            DropColumn("dbo.Budgets", "VendorId");
            DropColumn("dbo.Clients", "Title");
            DropTable("dbo.BudgetPayments");
        }
    }
}
