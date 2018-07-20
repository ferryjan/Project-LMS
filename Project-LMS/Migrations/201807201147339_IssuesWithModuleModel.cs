namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssuesWithModuleModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Modules", "Description", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Modules", "CourseName");
            DropColumn("dbo.Modules", "ModuleDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "ModuleDescription", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Modules", "CourseName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Modules", "Description");
            DropColumn("dbo.Modules", "Name");
        }
    }
}
