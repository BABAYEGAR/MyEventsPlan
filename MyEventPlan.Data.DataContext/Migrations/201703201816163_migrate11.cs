using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        BudgetId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        EstimatedAmount = c.Long(nullable: false),
                        NegotiatedAmount = c.Long(nullable: false),
                        ActualAmount = c.Long(nullable: false),
                        PaidTillDate = c.Long(nullable: false),
                        AmountStillDue = c.Long(nullable: false),
                        EventId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Budgets", "EventId", "dbo.Events");
            DropIndex("dbo.Budgets", new[] { "EventId" });
            DropTable("dbo.Budgets");
        }
    }
}
