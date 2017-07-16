namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate95 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "Title", c => c.String());
            AlterColumn("dbo.Clients", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "Title", c => c.String(nullable: false));
        }
    }
}
