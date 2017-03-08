namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.LocationId);
            
            AddColumn("dbo.Vendors", "BusinessName", c => c.String(nullable: false));
            AddColumn("dbo.Vendors", "BusinessContact", c => c.String(nullable: false));
            AddColumn("dbo.Vendors", "LocationId", c => c.Long());
            CreateIndex("dbo.Vendors", "LocationId");
            AddForeignKey("dbo.Vendors", "LocationId", "dbo.Locations", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vendors", "LocationId", "dbo.Locations");
            DropIndex("dbo.Vendors", new[] { "LocationId" });
            DropColumn("dbo.Vendors", "LocationId");
            DropColumn("dbo.Vendors", "BusinessContact");
            DropColumn("dbo.Vendors", "BusinessName");
            DropTable("dbo.Locations");
        }
    }
}
