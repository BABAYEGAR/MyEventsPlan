namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate29 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PasswordResets", "ConfirmPassword", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PasswordResets", "ConfirmPassword", c => c.String());
        }
    }
}
