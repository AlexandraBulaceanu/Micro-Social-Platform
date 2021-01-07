namespace Reaction.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Likes = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        PostId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        Likes = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ProfileId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.GroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        ProfileId = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        Email = c.String(),
                        Visibility = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Profile_ProfileId = c.Int(),
                    })
                .PrimaryKey(t => t.ProfileId)
                .ForeignKey("dbo.Profiles", t => t.Profile_ProfileId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Profile_ProfileId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        UserId = c.String(),
                        Visibility = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.ProfileFriends",
                c => new
                    {
                        Profile_ProfileId = c.Int(nullable: false),
                        Friend_FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Profile_ProfileId, t.Friend_FriendId })
                .ForeignKey("dbo.Profiles", t => t.Profile_ProfileId, cascadeDelete: true)
                .ForeignKey("dbo.Friends", t => t.Friend_FriendId, cascadeDelete: true)
                .Index(t => t.Profile_ProfileId)
                .Index(t => t.Friend_FriendId);
            
            CreateTable(
                "dbo.GroupProfiles",
                c => new
                    {
                        Group_GroupId = c.Int(nullable: false),
                        Profile_ProfileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_GroupId, t.Profile_ProfileId })
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Profiles", t => t.Profile_ProfileId, cascadeDelete: true)
                .Index(t => t.Group_GroupId)
                .Index(t => t.Profile_ProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Friends", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Profiles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.GroupProfiles", "Profile_ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.GroupProfiles", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.ProfileFriends", "Friend_FriendId", "dbo.Friends");
            DropForeignKey("dbo.ProfileFriends", "Profile_ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Profiles", "Profile_ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.GroupProfiles", new[] { "Profile_ProfileId" });
            DropIndex("dbo.GroupProfiles", new[] { "Group_GroupId" });
            DropIndex("dbo.ProfileFriends", new[] { "Friend_FriendId" });
            DropIndex("dbo.ProfileFriends", new[] { "Profile_ProfileId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Profiles", new[] { "Profile_ProfileId" });
            DropIndex("dbo.Profiles", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "GroupId" });
            DropIndex("dbo.Posts", new[] { "ProfileId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.GroupProfiles");
            DropTable("dbo.ProfileFriends");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Groups");
            DropTable("dbo.Profiles");
            DropTable("dbo.Friends");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
