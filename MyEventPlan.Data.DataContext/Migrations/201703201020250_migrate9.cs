namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventResourceMappings", "Quantity", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventResourceMappings", "Quantity");
        }
    }
}
