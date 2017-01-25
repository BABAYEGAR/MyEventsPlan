namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentietMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        RoleId = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.StaffId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.EventPlannerId);
            
            AddColumn("dbo.Events", "StaffId", c => c.Long());
            AddColumn("dbo.AppUsers", "StaffId", c => c.Long());
            CreateIndex("dbo.Events", "StaffId");
            CreateIndex("dbo.AppUsers", "StaffId");
            AddForeignKey("dbo.Events", "StaffId", "dbo.Staffs", "StaffId");
            AddForeignKey("dbo.AppUsers", "StaffId", "dbo.Staffs", "StaffId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Events", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Staffs", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Staffs", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.AppUsers", new[] { "StaffId" });
            DropIndex("dbo.Staffs", new[] { "EventPlannerId" });
            DropIndex("dbo.Staffs", new[] { "RoleId" });
            DropIndex("dbo.Events", new[] { "StaffId" });
            DropColumn("dbo.AppUsers", "StaffId");
            DropColumn("dbo.Events", "StaffId");
            DropTable("dbo.Staffs");
        }
    }
}
