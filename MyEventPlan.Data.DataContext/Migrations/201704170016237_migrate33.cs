using System.Data.Entity.Migrations;

namespace MyEventPlan.Data.DataContext.Migrations
{
    public partial class migrate33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "ManageEventVendors", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageCalendar", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageStaff", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageResources", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageGuestList", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageCheckList", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageBudgets", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageNotes", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageTasks", c => c.Boolean(nullable: false));
            DropColumn("dbo.Roles", "ManageEventPlanners");
            DropColumn("dbo.Roles", "ManageContracts");
            DropColumn("dbo.Roles", "ManageProposals");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "ManageProposals", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageContracts", c => c.Boolean(nullable: false));
            AddColumn("dbo.Roles", "ManageEventPlanners", c => c.Boolean(nullable: false));
            DropColumn("dbo.Roles", "ManageTasks");
            DropColumn("dbo.Roles", "ManageNotes");
            DropColumn("dbo.Roles", "ManageBudgets");
            DropColumn("dbo.Roles", "ManageCheckList");
            DropColumn("dbo.Roles", "ManageGuestList");
            DropColumn("dbo.Roles", "ManageResources");
            DropColumn("dbo.Roles", "ManageStaff");
            DropColumn("dbo.Roles", "ManageCalendar");
            DropColumn("dbo.Roles", "ManageEventVendors");
        }
    }
}
