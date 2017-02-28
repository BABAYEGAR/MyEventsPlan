namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "ManageContacts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "ManageContacts");
        }
    }
}