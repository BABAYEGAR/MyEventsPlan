namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate49 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorImages",
                c => new
                    {
                        VendorImageId = c.Long(nullable: false, identity: true),
                        ImageOne = c.String(),
                        ImageTwo = c.String(),
                        ImageThree = c.String(),
                        ImageFour = c.String(),
                        ImageFive = c.String(),
                        ImageSix = c.String(),
                        ImageSeven = c.String(),
                        ImageEight = c.String(),
                        ImageNine = c.String(),
                        ImageTen = c.String(),
                        VendorId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorImageId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.VendorId);
            
            AddColumn("dbo.Vendors", "Website", c => c.String());
            AddColumn("dbo.Vendors", "Logo", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VendorImages", "VendorId", "dbo.Vendors");
            DropIndex("dbo.VendorImages", new[] { "VendorId" });
            DropColumn("dbo.Vendors", "Logo");
            DropColumn("dbo.Vendors", "Website");
            DropTable("dbo.VendorImages");
        }
    }
}
