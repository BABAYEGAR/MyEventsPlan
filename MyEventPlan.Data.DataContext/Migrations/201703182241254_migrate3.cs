using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PasswordResets", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PasswordResets", "Email", c => c.String(nullable: false));
        }
    }
}
