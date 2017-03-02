namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CheckListItems", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CheckLists", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.GuestLists", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Notes", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Notes", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notes", "Content", c => c.String());
            AlterColumn("dbo.Notes", "Title", c => c.String());
            AlterColumn("dbo.GuestLists", "Name", c => c.String());
            AlterColumn("dbo.CheckLists", "Name", c => c.String());
            AlterColumn("dbo.CheckListItems", "Name", c => c.String());
        }
    }
}
