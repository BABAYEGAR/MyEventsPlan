namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PasswordResets", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PasswordResets", "Password", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
