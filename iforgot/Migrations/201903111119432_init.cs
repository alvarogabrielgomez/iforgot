namespace iforgot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.newUsers",
                c => new
                    {
                        newUser_id = c.Int(nullable: false, identity: true),
                        newUser_email = c.String(),
                        newUser_token = c.String(),
                        newUser_expires = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.newUser_id);
            
            CreateTable(
                "dbo.pwdReset",
                c => new
                    {
                        pwdReset_id = c.Int(nullable: false, identity: true),
                        pwdReset_email = c.String(),
                        pwdReset_selector = c.String(),
                        pwdReset_token = c.String(),
                        pwdReset_expires = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.pwdReset_id);
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        active = c.Boolean(nullable: false, defaultValue : false),
                        user_email = c.String(),
                        user_name = c.String(),
                        user_pwd = c.String(),
                        user_createdAt = c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    })
                .PrimaryKey(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.users");
            DropTable("dbo.pwdReset");
            DropTable("dbo.newUsers");
        }
    }
}
