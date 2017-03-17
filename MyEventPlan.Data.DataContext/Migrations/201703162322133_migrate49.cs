namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate49 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "ShowToClient", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "ShowToClient");
        }
    }
}
