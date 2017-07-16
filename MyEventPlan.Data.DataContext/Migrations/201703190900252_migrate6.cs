using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PasswordResets", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PasswordResets", "Password", c => c.String());
        }
    }
}
