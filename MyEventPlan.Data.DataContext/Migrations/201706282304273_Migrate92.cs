using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class Migrate92 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prospects", "ContactId", c => c.Long());
            CreateIndex("dbo.Prospects", "ContactId");
            AddForeignKey("dbo.Prospects", "ContactId", "dbo.Contacts", "ContactId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prospects", "ContactId", "dbo.Contacts");
            DropIndex("dbo.Prospects", new[] { "ContactId" });
            DropColumn("dbo.Prospects", "ContactId");
        }
    }
}
