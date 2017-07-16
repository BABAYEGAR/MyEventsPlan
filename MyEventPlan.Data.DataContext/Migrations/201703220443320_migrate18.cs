using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ItemDate = c.DateTime(nullable: false),
                        Qantity = c.Long(nullable: false),
                        UnitCost = c.Long(nullable: false),
                        InvoiceId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.InvoiceItemId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.InvoicePayments",
                c => new
                    {
                        InvoicePaymentId = c.Long(nullable: false, identity: true),
                        Amount = c.Long(nullable: false),
                        Reference = c.Long(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        InvoiceId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.InvoicePaymentId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.PersonalCheckListItems",
                c => new
                    {
                        PersonalCheckListItemId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Checked = c.Boolean(nullable: false),
                        EventId = c.Long(nullable: false),
                        PersonalCheckListId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.PersonalCheckListItemId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.PersonalCheckLists", t => t.PersonalCheckListId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.PersonalCheckListId);
            
            CreateTable(
                "dbo.PersonalCheckLists",
                c => new
                    {
                        PersonalCheckListId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Status = c.String(),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.PersonalCheckListId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId);
            
            AddColumn("dbo.Invoices", "EventId", c => c.Long());
            CreateIndex("dbo.Invoices", "EventId");
            AddForeignKey("dbo.Invoices", "EventId", "dbo.Events", "EventId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonalCheckListItems", "PersonalCheckListId", "dbo.PersonalCheckLists");
            DropForeignKey("dbo.PersonalCheckLists", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.PersonalCheckListItems", "EventId", "dbo.Events");
            DropForeignKey("dbo.InvoicePayments", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "EventId", "dbo.Events");
            DropIndex("dbo.PersonalCheckLists", new[] { "AppUserId" });
            DropIndex("dbo.PersonalCheckListItems", new[] { "PersonalCheckListId" });
            DropIndex("dbo.PersonalCheckListItems", new[] { "EventId" });
            DropIndex("dbo.InvoicePayments", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", new[] { "EventId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropColumn("dbo.Invoices", "EventId");
            DropTable("dbo.PersonalCheckLists");
            DropTable("dbo.PersonalCheckListItems");
            DropTable("dbo.InvoicePayments");
            DropTable("dbo.InvoiceItems");
        }
    }
}
