namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
