namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twentyFirstMigrate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Staffs", "Password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Staffs", "Password", c => c.String(nullable: false));
        }
    }
}
