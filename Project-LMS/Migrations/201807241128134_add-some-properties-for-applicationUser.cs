namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsomepropertiesforapplicationUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "GivenName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "FamilyName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "FamilyName", c => c.String());
            AlterColumn("dbo.AspNetUsers", "GivenName", c => c.String());
        }
    }
}
