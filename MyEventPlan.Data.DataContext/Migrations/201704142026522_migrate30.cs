using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate30 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs");
            DropIndex("dbo.EventResourceMappings", new[] { "ResourceId" });
            DropIndex("dbo.StaffEventMappings", new[] { "StaffId" });
            AlterColumn("dbo.EventResourceMappings", "ResourceId", c => c.Long(nullable: false));
            AlterColumn("dbo.StaffEventMappings", "StaffId", c => c.Long(nullable: false));
            CreateIndex("dbo.EventResourceMappings", "ResourceId");
            CreateIndex("dbo.StaffEventMappings", "StaffId");
            AddForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources", "ResourceId", cascadeDelete: true);
            AddForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs", "StaffId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources");
            DropIndex("dbo.StaffEventMappings", new[] { "StaffId" });
            DropIndex("dbo.EventResourceMappings", new[] { "ResourceId" });
            AlterColumn("dbo.StaffEventMappings", "StaffId", c => c.Long());
            AlterColumn("dbo.EventResourceMappings", "ResourceId", c => c.Long());
            CreateIndex("dbo.StaffEventMappings", "StaffId");
            CreateIndex("dbo.EventResourceMappings", "ResourceId");
            AddForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs", "StaffId");
            AddForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources", "ResourceId");
        }
    }
}
