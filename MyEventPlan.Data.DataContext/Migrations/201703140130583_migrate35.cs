namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate35 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Password", c => c.String());
            AlterColumn("dbo.Vendors", "ConfirmPassword", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "ConfirmPassword", c => c.String(nullable: false));
            AlterColumn("dbo.Vendors", "Password", c => c.String(nullable: false));
        }
    }
}
