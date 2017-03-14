namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate46 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "Body", c => c.String());
            AlterColumn("dbo.Messages", "Subject", c => c.String());
        }
    }
}
