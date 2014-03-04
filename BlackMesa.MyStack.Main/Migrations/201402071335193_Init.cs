namespace BlackMesa.MyStack.Main.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyStack_Cards",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        DateEdited = c.DateTime(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                        Position = c.Int(nullable: false),
                        FolderId = c.Guid(nullable: false),
                        FrontSide = c.String(),
                        BackSide = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyStack_Folders", t => t.FolderId, cascadeDelete: true)
                .ForeignKey("dbo.Identity_Users", t => t.OwnerId)
                .Index(t => t.FolderId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.MyStack_Folders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerId = c.String(maxLength: 128),
                        Name = c.String(nullable: false),
                        Level = c.Int(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                        ParentFolder_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Identity_Users", t => t.OwnerId)
                .ForeignKey("dbo.MyStack_Folders", t => t.ParentFolder_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.ParentFolder_Id);
            
            CreateTable(
                "dbo.Identity_Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Identity_UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Identity_Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Identity_UserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.Identity_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Identity_UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Identity_Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Identity_Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Identity_Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MyStack_TestItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Result = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CardId = c.Guid(nullable: false),
                        TestId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MyStack_Cards", t => t.CardId, cascadeDelete: true)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyStack_TestItems", "CardId", "dbo.MyStack_Cards");
            DropForeignKey("dbo.MyStack_Cards", "OwnerId", "dbo.Identity_Users");
            DropForeignKey("dbo.MyStack_Folders", "ParentFolder_Id", "dbo.MyStack_Folders");
            DropForeignKey("dbo.MyStack_Folders", "OwnerId", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserClaims", "User_Id", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "UserId", "dbo.Identity_Users");
            DropForeignKey("dbo.Identity_UserRoles", "RoleId", "dbo.Identity_Roles");
            DropForeignKey("dbo.Identity_UserLogins", "UserId", "dbo.Identity_Users");
            DropForeignKey("dbo.MyStack_Cards", "FolderId", "dbo.MyStack_Folders");
            DropIndex("dbo.MyStack_TestItems", new[] { "CardId" });
            DropIndex("dbo.MyStack_Cards", new[] { "OwnerId" });
            DropIndex("dbo.MyStack_Folders", new[] { "ParentFolder_Id" });
            DropIndex("dbo.MyStack_Folders", new[] { "OwnerId" });
            DropIndex("dbo.Identity_UserClaims", new[] { "User_Id" });
            DropIndex("dbo.Identity_UserRoles", new[] { "UserId" });
            DropIndex("dbo.Identity_UserRoles", new[] { "RoleId" });
            DropIndex("dbo.Identity_UserLogins", new[] { "UserId" });
            DropIndex("dbo.MyStack_Cards", new[] { "FolderId" });
            DropTable("dbo.MyStack_TestItems");
            DropTable("dbo.Identity_Roles");
            DropTable("dbo.Identity_UserRoles");
            DropTable("dbo.Identity_UserLogins");
            DropTable("dbo.Identity_UserClaims");
            DropTable("dbo.Identity_Users");
            DropTable("dbo.MyStack_Folders");
            DropTable("dbo.MyStack_Cards");
        }
    }
}
