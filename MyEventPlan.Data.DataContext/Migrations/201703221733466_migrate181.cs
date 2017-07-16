using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate181 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceItems", "Quantity", c => c.Long(nullable: false));
            DropColumn("dbo.InvoiceItems", "Qantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InvoiceItems", "Qantity", c => c.Long(nullable: false));
            DropColumn("dbo.InvoiceItems", "Quantity");
        }
    }
}
