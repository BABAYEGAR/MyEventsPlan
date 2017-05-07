namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate44 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorPackages", "PackageGrade", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorPackages", "PackageGrade");
        }
    }
}
