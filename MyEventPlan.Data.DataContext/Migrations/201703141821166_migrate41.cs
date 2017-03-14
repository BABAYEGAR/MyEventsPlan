namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate41 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guests", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Guests", "Status");
        }
    }
}
