namespace MyEventPlan.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        EventId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EndTime = c.String(nullable: false),
                        Location = c.String(),
                        Notes = c.String(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        Status = c.String(),
                        EventTypeId = c.Long(nullable: false),
                        TargetBudget = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.String(),
                        EndDate = c.DateTime(nullable: false),
                        EndTime = c.String(),
                        EventPlannerId = c.Long(),
                        StaffId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffId)
                .Index(t => t.EventTypeId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.EventPlanners",
                c => new
                    {
                        EventPlannerId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        ConfirmPassword = c.String(nullable: false),
                        RoleId = c.Long(),
                    })
                .PrimaryKey(t => t.EventPlannerId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ManageApplicationUser = c.Boolean(nullable: false),
                        ManageRoles = c.Boolean(nullable: false),
                        ManageEvents = c.Boolean(nullable: false),
                        ManageEventType = c.Boolean(nullable: false),
                        ManageEventPlanners = c.Boolean(nullable: false),
                        ManageVendors = c.Boolean(nullable: false),
                        ManageVendorServices = c.Boolean(nullable: false),
                        ManageProspects = c.Boolean(nullable: false),
                        ManageInvoices = c.Boolean(nullable: false),
                        ManageContracts = c.Boolean(nullable: false),
                        ManageProposals = c.Boolean(nullable: false),
                        ManageContacts = c.Boolean(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        EventTypeId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventTypeId);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        Password = c.String(),
                        Status = c.String(),
                        RoleId = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.StaffId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        AppUserId = c.Long(nullable: false, identity: true),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false),
                        RoleId = c.Long(),
                        EventPlannerId = c.Long(),
                        VendorId = c.Long(),
                        StaffId = c.Long(),
                        ClientId = c.Long(),
                        ProfileImage = c.String(),
                        Status = c.String(),
                        Verified = c.Boolean(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.AppUserId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Staffs", t => t.StaffId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.RoleId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.VendorId)
                .Index(t => t.StaffId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        EventPlannerId = c.Long(),
                        EventId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                        Address = c.String(nullable: false),
                        About = c.String(nullable: false),
                        ImageOne = c.String(),
                        ImageTwo = c.String(),
                        ImageThree = c.String(),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        BusinessName = c.String(nullable: false),
                        BusinessContact = c.String(nullable: false),
                        VendorServiceId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        LocationId = c.Long(),
                        EventId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.VendorServices", t => t.VendorServiceId, cascadeDelete: false)
                .Index(t => t.VendorServiceId)
                .Index(t => t.EventPlannerId)
                .Index(t => t.LocationId)
                .Index(t => t.EventId);
            
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
            
            CreateTable(
                "dbo.VendorServices",
                c => new
                    {
                        VendorServiceId = c.Long(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false),
                        Scale = c.String(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.VendorServiceId);
            
            CreateTable(
                "dbo.CheckListItems",
                c => new
                    {
                        CheckListItemId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Checked = c.Boolean(nullable: false),
                        EventId = c.Long(nullable: false),
                        CheckListId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.CheckListItemId)
                .ForeignKey("dbo.CheckLists", t => t.CheckListId, cascadeDelete: false)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.CheckListId);
            
            CreateTable(
                "dbo.CheckLists",
                c => new
                    {
                        CheckListId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Status = c.String(),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.CheckListId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Firstname = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Mobile = c.String(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ContactId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.ContactRoles",
                c => new
                    {
                        ContactRoleId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        RoleType = c.String(),
                        Employee = c.Boolean(nullable: false),
                        Client = c.Boolean(nullable: false),
                        Administrator = c.Boolean(nullable: false),
                        MasterAdministrator = c.Boolean(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ContactRoleId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.EventContactMappings",
                c => new
                    {
                        EventContactMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        ContactId = c.Long(),
                    })
                .PrimaryKey(t => t.EventContactMappingId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.EventResourceMappings",
                c => new
                    {
                        EventResourceMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        ResourceId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventResourceMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.Resources", t => t.ResourceId)
                .Index(t => t.EventId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Quantity = c.Long(nullable: false),
                        EventPlannerId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: false)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.EventVendorMappings",
                c => new
                    {
                        EventVendorMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        VendorId = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.EventVendorMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.EventId)
                .Index(t => t.VendorId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.GuestLists",
                c => new
                    {
                        GuestListId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EventId = c.Long(nullable: false),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.GuestListId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .Index(t => t.EventId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Status = c.String(),
                        EventId = c.Long(nullable: false),
                        GuestListId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.GuestId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .ForeignKey("dbo.GuestLists", t => t.GuestListId, cascadeDelete: false)
                .Index(t => t.EventId)
                .Index(t => t.GuestListId);
            
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
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: false)
                .ForeignKey("dbo.MessageGroups", t => t.MessageGroupId, cascadeDelete: false)
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
                        Subject = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Read = c.Boolean(nullable: false),
                        AttachedFile = c.String(),
                        Sender = c.Long(),
                        AppUserId = c.Long(nullable: false),
                        MessageGroupId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: false)
                .ForeignKey("dbo.MessageGroups", t => t.MessageGroupId)
                .Index(t => t.AppUserId)
                .Index(t => t.MessageGroupId);
            
            CreateTable(
                "dbo.NewsActions",
                c => new
                    {
                        NewsActionId = c.Long(nullable: false, identity: true),
                        Action = c.String(),
                        NewsId = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NewsActionId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: false)
                .ForeignKey("dbo.News", t => t.NewsId, cascadeDelete: false)
                .Index(t => t.NewsId)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Long(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        NewsImage = c.String(),
                        EventPlannerId = c.Long(nullable: false),
                        Likes = c.Long(nullable: false),
                        Dislike = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId, cascadeDelete: false)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        NoteId = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        ShowToClient = c.Boolean(nullable: false),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Long(nullable: false, identity: true),
                        Message = c.String(),
                        NotificationType = c.String(),
                        Read = c.Boolean(nullable: false),
                        NotificationKey = c.Long(nullable: false),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: false)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.ProspectContactMappings",
                c => new
                    {
                        ProspectContactMappingId = c.Long(nullable: false, identity: true),
                        ProspectId = c.Long(),
                        ContactId = c.Long(),
                    })
                .PrimaryKey(t => t.ProspectContactMappingId)
                .ForeignKey("dbo.Contacts", t => t.ContactId)
                .ForeignKey("dbo.Prospects", t => t.ProspectId)
                .Index(t => t.ProspectId)
                .Index(t => t.ContactId);
            
            CreateTable(
                "dbo.Prospects",
                c => new
                    {
                        ProspectId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        EventTypeId = c.Long(nullable: false),
                        TargetBudget = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        StartTime = c.String(),
                        EndDate = c.DateTime(nullable: false),
                        EndTime = c.String(),
                        EventPlannerId = c.Long(),
                        Status = c.String(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.ProspectId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId, cascadeDelete: false)
                .Index(t => t.EventTypeId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingId = c.Long(nullable: false, identity: true),
                        BackgroundColor = c.String(),
                        AppUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.SettingId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: false)
                .Index(t => t.AppUserId);
            
            CreateTable(
                "dbo.StaffEventMappings",
                c => new
                    {
                        StaffEventMappingId = c.Long(nullable: false, identity: true),
                        EventId = c.Long(),
                        StaffId = c.Long(),
                        EventPlannerId = c.Long(),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.StaffEventMappingId)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.EventPlanners", t => t.EventPlannerId)
                .ForeignKey("dbo.Staffs", t => t.StaffId)
                .Index(t => t.EventId)
                .Index(t => t.StaffId)
                .Index(t => t.EventPlannerId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Status = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        EventId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: false)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "EventId", "dbo.Events");
            DropForeignKey("dbo.StaffEventMappings", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.StaffEventMappings", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.StaffEventMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.Settings", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.ProspectContactMappings", "ProspectId", "dbo.Prospects");
            DropForeignKey("dbo.Prospects", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.Prospects", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.ProspectContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.Notifications", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Notes", "EventId", "dbo.Events");
            DropForeignKey("dbo.NewsActions", "NewsId", "dbo.News");
            DropForeignKey("dbo.News", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.NewsActions", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Messages", "MessageGroupId", "dbo.MessageGroups");
            DropForeignKey("dbo.Messages", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.MessageGroupMembers", "MessageGroupId", "dbo.MessageGroups");
            DropForeignKey("dbo.MessageGroupMembers", "AppUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Guests", "GuestListId", "dbo.GuestLists");
            DropForeignKey("dbo.Guests", "EventId", "dbo.Events");
            DropForeignKey("dbo.GuestLists", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.GuestLists", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventVendorMappings", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.EventVendorMappings", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventVendorMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventResourceMappings", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Resources", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventResourceMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventContactMappings", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventContactMappings", "ContactId", "dbo.Contacts");
            DropForeignKey("dbo.ContactRoles", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Contacts", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.CheckListItems", "EventId", "dbo.Events");
            DropForeignKey("dbo.CheckListItems", "CheckListId", "dbo.CheckLists");
            DropForeignKey("dbo.CheckLists", "EventId", "dbo.Events");
            DropForeignKey("dbo.AppUsers", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Vendors", "VendorServiceId", "dbo.VendorServices");
            DropForeignKey("dbo.Vendors", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Vendors", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Vendors", "EventId", "dbo.Events");
            DropForeignKey("dbo.AppUsers", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.AppUsers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AppUsers", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.AppUsers", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Clients", "EventId", "dbo.Events");
            DropForeignKey("dbo.Appointments", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Appointments", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Staffs", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Staffs", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.Events", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.Events", "EventPlannerId", "dbo.EventPlanners");
            DropForeignKey("dbo.EventPlanners", "RoleId", "dbo.Roles");
            DropIndex("dbo.Tasks", new[] { "EventId" });
            DropIndex("dbo.StaffEventMappings", new[] { "EventPlannerId" });
            DropIndex("dbo.StaffEventMappings", new[] { "StaffId" });
            DropIndex("dbo.StaffEventMappings", new[] { "EventId" });
            DropIndex("dbo.Settings", new[] { "AppUserId" });
            DropIndex("dbo.Prospects", new[] { "EventPlannerId" });
            DropIndex("dbo.Prospects", new[] { "EventTypeId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ContactId" });
            DropIndex("dbo.ProspectContactMappings", new[] { "ProspectId" });
            DropIndex("dbo.Notifications", new[] { "AppUserId" });
            DropIndex("dbo.Notes", new[] { "EventId" });
            DropIndex("dbo.News", new[] { "EventPlannerId" });
            DropIndex("dbo.NewsActions", new[] { "AppUserId" });
            DropIndex("dbo.NewsActions", new[] { "NewsId" });
            DropIndex("dbo.Messages", new[] { "MessageGroupId" });
            DropIndex("dbo.Messages", new[] { "AppUserId" });
            DropIndex("dbo.MessageGroupMembers", new[] { "AppUserId" });
            DropIndex("dbo.MessageGroupMembers", new[] { "MessageGroupId" });
            DropIndex("dbo.Guests", new[] { "GuestListId" });
            DropIndex("dbo.Guests", new[] { "EventId" });
            DropIndex("dbo.GuestLists", new[] { "EventPlannerId" });
            DropIndex("dbo.GuestLists", new[] { "EventId" });
            DropIndex("dbo.EventVendorMappings", new[] { "EventPlannerId" });
            DropIndex("dbo.EventVendorMappings", new[] { "VendorId" });
            DropIndex("dbo.EventVendorMappings", new[] { "EventId" });
            DropIndex("dbo.Resources", new[] { "EventPlannerId" });
            DropIndex("dbo.EventResourceMappings", new[] { "ResourceId" });
            DropIndex("dbo.EventResourceMappings", new[] { "EventId" });
            DropIndex("dbo.EventContactMappings", new[] { "ContactId" });
            DropIndex("dbo.EventContactMappings", new[] { "EventId" });
            DropIndex("dbo.ContactRoles", new[] { "EventPlannerId" });
            DropIndex("dbo.Contacts", new[] { "EventPlannerId" });
            DropIndex("dbo.CheckLists", new[] { "EventId" });
            DropIndex("dbo.CheckListItems", new[] { "CheckListId" });
            DropIndex("dbo.CheckListItems", new[] { "EventId" });
            DropIndex("dbo.Vendors", new[] { "EventId" });
            DropIndex("dbo.Vendors", new[] { "LocationId" });
            DropIndex("dbo.Vendors", new[] { "EventPlannerId" });
            DropIndex("dbo.Vendors", new[] { "VendorServiceId" });
            DropIndex("dbo.Clients", new[] { "EventId" });
            DropIndex("dbo.Clients", new[] { "EventPlannerId" });
            DropIndex("dbo.AppUsers", new[] { "ClientId" });
            DropIndex("dbo.AppUsers", new[] { "StaffId" });
            DropIndex("dbo.AppUsers", new[] { "VendorId" });
            DropIndex("dbo.AppUsers", new[] { "EventPlannerId" });
            DropIndex("dbo.AppUsers", new[] { "RoleId" });
            DropIndex("dbo.Staffs", new[] { "EventPlannerId" });
            DropIndex("dbo.Staffs", new[] { "RoleId" });
            DropIndex("dbo.EventPlanners", new[] { "RoleId" });
            DropIndex("dbo.Events", new[] { "StaffId" });
            DropIndex("dbo.Events", new[] { "EventPlannerId" });
            DropIndex("dbo.Events", new[] { "EventTypeId" });
            DropIndex("dbo.Appointments", new[] { "EventPlannerId" });
            DropIndex("dbo.Appointments", new[] { "EventId" });
            DropTable("dbo.Tasks");
            DropTable("dbo.StaffEventMappings");
            DropTable("dbo.Settings");
            DropTable("dbo.Prospects");
            DropTable("dbo.ProspectContactMappings");
            DropTable("dbo.Notifications");
            DropTable("dbo.Notes");
            DropTable("dbo.News");
            DropTable("dbo.NewsActions");
            DropTable("dbo.Messages");
            DropTable("dbo.MessageGroups");
            DropTable("dbo.MessageGroupMembers");
            DropTable("dbo.Guests");
            DropTable("dbo.GuestLists");
            DropTable("dbo.EventVendorMappings");
            DropTable("dbo.Resources");
            DropTable("dbo.EventResourceMappings");
            DropTable("dbo.EventContactMappings");
            DropTable("dbo.ContactRoles");
            DropTable("dbo.Contacts");
            DropTable("dbo.CheckLists");
            DropTable("dbo.CheckListItems");
            DropTable("dbo.VendorServices");
            DropTable("dbo.Locations");
            DropTable("dbo.Vendors");
            DropTable("dbo.Clients");
            DropTable("dbo.AppUsers");
            DropTable("dbo.Staffs");
            DropTable("dbo.EventTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.EventPlanners");
            DropTable("dbo.Events");
            DropTable("dbo.Appointments");
        }
    }
}
