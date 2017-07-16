using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate67 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventPlanners", "FacebookPage", c => c.String());
            AddColumn("dbo.EventPlanners", "TwitterPage", c => c.String());
            AddColumn("dbo.EventPlanners", "InstagramPage", c => c.String());
            AddColumn("dbo.EventPlanners", "GooglePlusPage", c => c.String());
            AddColumn("dbo.EventPlanners", "YoutubePage", c => c.String());
            AddColumn("dbo.EventPlanners", "Website", c => c.String());
            AddColumn("dbo.EventPlanners", "About", c => c.String());
            AddColumn("dbo.EventPlanners", "Logo", c => c.String());
            AddColumn("dbo.EventPlanners", "MinimumPrice", c => c.Long());
            AddColumn("dbo.EventPlanners", "MaximumPrice", c => c.Long());
            AddColumn("dbo.EventPlanners", "PricingDetails", c => c.String());
            DropColumn("dbo.AppUsers", "ProfileImage");
            DropColumn("dbo.AppUsers", "Verified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppUsers", "Verified", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "ProfileImage", c => c.String());
            DropColumn("dbo.EventPlanners", "PricingDetails");
            DropColumn("dbo.EventPlanners", "MaximumPrice");
            DropColumn("dbo.EventPlanners", "MinimumPrice");
            DropColumn("dbo.EventPlanners", "Logo");
            DropColumn("dbo.EventPlanners", "About");
            DropColumn("dbo.EventPlanners", "Website");
            DropColumn("dbo.EventPlanners", "YoutubePage");
            DropColumn("dbo.EventPlanners", "GooglePlusPage");
            DropColumn("dbo.EventPlanners", "InstagramPage");
            DropColumn("dbo.EventPlanners", "TwitterPage");
            DropColumn("dbo.EventPlanners", "FacebookPage");
        }
    }
}
