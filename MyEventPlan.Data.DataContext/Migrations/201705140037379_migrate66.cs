using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate66 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventPlannerEnquiries",
                c => new
                    {
                        EventPlannerEnquiryId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        Note = c.String(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.EventPlannerEnquiryId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.EventPlannerReviews",
                c => new
                    {
                        EventPlannerReviewId = c.Long(nullable: false, identity: true),
                        ReviewerName = c.String(nullable: false),
                        ReviewerEmail = c.String(nullable: false),
                        ReviewTitle = c.String(nullable: false),
                        ReviewBody = c.String(nullable: false),
                        Rating = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long()
                    })
                .PrimaryKey(t => t.EventPlannerReviewId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventPlannerReviews", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlannerEnquiries", "EventPlannerId", "dbo.EventPlanners");
            DropIndex("dbo.EventPlannerReviews", new[] { "EventPlannerId" });
            DropIndex("dbo.EventPlannerEnquiries", new[] { "EventPlannerId" });
            DropTable("dbo.EventPlannerReviews");
            DropTable("dbo.EventPlannerEnquiries");
        }
    }
}
