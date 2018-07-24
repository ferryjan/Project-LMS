namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removesomepropertiesforapplicationUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
