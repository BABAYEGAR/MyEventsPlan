using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "StaffId", c => c.Long(nullable: true));
            CreateIndex("dbo.Tasks", "StaffId");
            AddForeignKey("dbo.Tasks", "StaffId", "dbo.Staffs", "StaffId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "StaffId", "dbo.Staffs");
            DropIndex("dbo.Tasks", new[] { "StaffId" });
            DropColumn("dbo.Tasks", "StaffId");
        }
    }
}
