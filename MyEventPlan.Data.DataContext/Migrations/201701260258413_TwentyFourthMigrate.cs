namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentyFourthMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventVendorMappings",
                c => new
                    {
                        EventVendorMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        VendorId = c.Long(),
                    })
                .PrimaryKey(t => t.EventVendorMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.EventId)
                .Index(t => t.VendorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventVendorMappings", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.EventVendorMappings", "EventId", "dbo.Events");
            DropIndex("dbo.EventVendorMappings", new[] { "VendorId" });
            DropIndex("dbo.EventVendorMappings", new[] { "EventId" });
            DropTable("dbo.EventVendorMappings");
        }
    }
}
