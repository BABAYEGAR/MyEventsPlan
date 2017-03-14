namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate42 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageGroupMembers",
                c => new
                    {
                        MessageGroupMemberId = c.Long(nullable: false, identity: true),
                        MessageGroupId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.MessageGroupMemberId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .ForeignKey("dbo.MessageGroups", t => t.MessageGroupId, cascadeDelete: true)
                .Index(t => t.MessageGroupId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.MessageGroups",
                c => new
                    {
                        MessageGroupId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.MessageGroupId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Long(nullable: false, identity: true),
                        Subject = c.String(),
                        AttachedFile = c.String(),
                        Sender = c.Long(),
                        Receipient = c.Long(),
                        MessageGroupId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.MessageGroups", t => t.MessageGroupId)
                .Index(t => t.MessageGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "MessageGroupId", "dbo.MessageGroups");
            DropForeignKey("dbo.MessageGroupMembers", "MessageGroupId", "dbo.MessageGroups");
            DropForeignKey("dbo.MessageGroupMembers", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.Messages", new[] { "MessageGroupId" });
            DropIndex("dbo.MessageGroupMembers", new[] { "AppUserId" });
            DropIndex("dbo.MessageGroupMembers", new[] { "MessageGroupId" });
            DropTable("dbo.Messages");
            DropTable("dbo.MessageGroups");
            DropTable("dbo.MessageGroupMembers");
        }
    }
}
