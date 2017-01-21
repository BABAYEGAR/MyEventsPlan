namespace MyEventPlan.Data.DataContext.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class migration3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prospects", "ContactRoleId", "dbo.ContactRoles");
            DropIndex("dbo.Prospects", new[] { "ContactRoleId" });
            DropColumn("dbo.Prospects", "ContactRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prospects", "ContactRoleId", c => c.Long(nullable: false));
            CreateIndex("dbo.Prospects", "ContactRoleId");
            AddForeignKey("dbo.Prospects", "ContactRoleId", "dbo.ContactRoles", "ContactRoleId", cascadeDelete: true);
        }
    }
}
