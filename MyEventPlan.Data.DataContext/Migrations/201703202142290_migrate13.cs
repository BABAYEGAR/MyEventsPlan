using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Long(nullable: false, identity: true),
                        InvoiceName = c.String(nullable: false),
                        InvoiceNumber = c.Long(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ClientId = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.EventPlannerId);
            
            DropColumn("dbo.Clients", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Password", c => c.String());
            DropForeignKey("dbo.Invoices", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Invoices", "ClientId", "dbo.Clients");
            DropIndex("dbo.Invoices", new[] { "EventPlannerId" });
            DropIndex("dbo.Invoices", new[] { "ClientId" });
            DropTable("dbo.Invoices");
        }
    }
}
