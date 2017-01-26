namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentySeventhMigrate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staffs", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "Status");
        }
    }
}
