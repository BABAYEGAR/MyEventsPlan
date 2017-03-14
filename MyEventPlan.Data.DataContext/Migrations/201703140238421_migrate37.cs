namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "ClientId", c => c.Long());
            CreateIndex("dbo.AppUsers", "ClientId");
            AddForeignKey("dbo.AppUsers", "ClientId", "dbo.Clients", "ClientId");
            DropColumn("dbo.Clients", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "ConfirmPassword", c => c.String());
            DropForeignKey("dbo.AppUsers", "ClientId", "dbo.Clients");
            DropIndex("dbo.AppUsers", new[] { "ClientId" });
            DropColumn("dbo.AppUsers", "ClientId");
        }
    }
}
