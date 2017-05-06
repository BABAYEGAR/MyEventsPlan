namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate35 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorPackageItems",
                c => new
                    {
                        VendorPackageItemId = c.Long(nullable: false, identity: true),
                        ItemName = c.String(nullable: false),
                        Amount = c.Long(nullable: false),
                        VendorPackageId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorPackageItemId)
                .ForeignKey("dbo.VendorPackages", t => t.VendorPackageId, cascadeDelete: true)
                .Index(t => t.VendorPackageId);
            
            CreateTable(
                "dbo.VendorPackages",
                c => new
                    {
                        VendorPackageId = c.Long(nullable: false, identity: true),
                        PackageName = c.String(nullable: false),
                        Amount = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorPackageId);
            
            CreateTable(
                "dbo.VendorPackageSettings",
                c => new
                    {
                        VendorPackageSettingId = c.Long(nullable: false, identity: true),
                        VendorPackageId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        VendorId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorPackageSettingId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .ForeignKey("dbo.VendorPackages", t => t.VendorPackageId, cascadeDelete: true)
                .Index(t => t.VendorPackageId)
                .Index(t => t.VendorId)
                .Index(t => t.AppUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VendorPackageSettings", "VendorPackageId", "dbo.VendorPackages");
            DropForeignKey("dbo.VendorPackageSettings", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.VendorPackageSettings", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.VendorPackageItems", "VendorPackageId", "dbo.VendorPackages");
            DropIndex("dbo.VendorPackageSettings", new[] { "AppUserId" });
            DropIndex("dbo.VendorPackageSettings", new[] { "VendorId" });
            DropIndex("dbo.VendorPackageSettings", new[] { "VendorPackageId" });
            DropIndex("dbo.VendorPackageItems", new[] { "VendorPackageId" });
            DropTable("dbo.VendorPackageSettings");
            DropTable("dbo.VendorPackages");
            DropTable("dbo.VendorPackageItems");
        }
    }
}
