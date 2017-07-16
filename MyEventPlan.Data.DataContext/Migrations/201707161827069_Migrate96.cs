namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate96 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "AccessEmail", c => c.String());
            AddColumn("dbo.Clients", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Password");
            DropColumn("dbo.Clients", "AccessEmail");
        }
    }
}
