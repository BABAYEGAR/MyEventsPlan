namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate38 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clients", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "Address", c => c.String(nullable: false));
        }
    }
}
