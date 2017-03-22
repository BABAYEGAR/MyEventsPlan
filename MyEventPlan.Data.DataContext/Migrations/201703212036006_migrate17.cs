namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageId = c.Long(nullable: false, identity: true),
                        PackageName = c.String(nullable: false),
                        Amount = c.String(),
                        MaximumEvents = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.PackageId);
            
            AddColumn("dbo.Roles", "ManagePackages", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageLocations", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "ManageLocations");
            DropColumn("dbo.Roles", "ManagePackages");
            DropTable("dbo.Packages");
        }
    }
}
