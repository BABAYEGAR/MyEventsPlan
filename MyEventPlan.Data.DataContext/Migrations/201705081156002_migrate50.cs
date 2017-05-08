namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate50 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Password", c => c.String(nullable: false));
            AlterColumn("dbo.Vendors", "ConfirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "ConfirmPassword", c => c.String());
            AlterColumn("dbo.Vendors", "Password", c => c.String());
        }
    }
}
