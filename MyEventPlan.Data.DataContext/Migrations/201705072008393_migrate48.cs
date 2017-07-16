using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate48 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorEnquiries",
                c => new
                    {
                        VendorEnquiryId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        Note = c.String(),
                        VendorId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.VendorEnquiryId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.VendorReviews",
                c => new
                    {
                        VendorReviewId = c.Long(nullable: false, identity: true),
                        ReviewerName = c.String(nullable: false),
                        ReviewerEmail = c.String(nullable: false),
                        ReviewTitle = c.String(nullable: false),
                        ReviewBody = c.String(nullable: false),
                        Rating = c.String(),
                        VendorId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.VendorReviewId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.VendorId);
            
            AddColumn("dbo.Vendors", "FacebookPage", c => c.String());
            AddColumn("dbo.Vendors", "TwitterPage", c => c.String());
            AddColumn("dbo.Vendors", "InstagramPage", c => c.String());
            AddColumn("dbo.Vendors", "GooglePlusPage", c => c.String());
            AddColumn("dbo.Vendors", "YoutubePage", c => c.String());
            AddColumn("dbo.Vendors", "AveragePrice", c => c.Long(nullable: false));
            AddColumn("dbo.Vendors", "PricingDetails", c => c.Long(nullable: false));
            DropColumn("dbo.Vendors", "MinimumPrice");
            DropColumn("dbo.Vendors", "ImageOne");
            DropColumn("dbo.Vendors", "ImageTwo");
            DropColumn("dbo.Vendors", "ImageThree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "ImageThree", c => c.String());
            AddColumn("dbo.Vendors", "ImageTwo", c => c.String());
            AddColumn("dbo.Vendors", "ImageOne", c => c.String());
            AddColumn("dbo.Vendors", "MinimumPrice", c => c.Long(nullable: false));
            DropForeignKey("dbo.VendorReviews", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.VendorEnquiries", "VendorId", "dbo.Vendors");
            DropIndex("dbo.VendorReviews", new[] { "VendorId" });
            DropIndex("dbo.VendorEnquiries", new[] { "VendorId" });
            DropColumn("dbo.Vendors", "PricingDetails");
            DropColumn("dbo.Vendors", "AveragePrice");
            DropColumn("dbo.Vendors", "YoutubePage");
            DropColumn("dbo.Vendors", "GooglePlusPage");
            DropColumn("dbo.Vendors", "InstagramPage");
            DropColumn("dbo.Vendors", "TwitterPage");
            DropColumn("dbo.Vendors", "FacebookPage");
            DropTable("dbo.VendorReviews");
            DropTable("dbo.VendorEnquiries");
        }
    }
}
