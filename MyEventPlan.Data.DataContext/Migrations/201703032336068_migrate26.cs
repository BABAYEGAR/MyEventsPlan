namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate26 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Content", c => c.String(nullable: true));
            AlterColumn("dbo.Tasks", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Tasks", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "Description", c => c.String());
            AlterColumn("dbo.Tasks", "Name", c => c.String());
            AlterColumn("dbo.News", "Content", c => c.String());
        }
    }
}
