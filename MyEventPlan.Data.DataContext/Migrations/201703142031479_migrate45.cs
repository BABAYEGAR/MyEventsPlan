namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Body", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "Body");
        }
    }
}
