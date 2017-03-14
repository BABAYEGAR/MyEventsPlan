namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CheckLists", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CheckLists", "Status");
        }
    }
}
