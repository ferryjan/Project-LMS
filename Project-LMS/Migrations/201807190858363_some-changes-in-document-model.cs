namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somechangesindocumentmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Documents", "CourseName");
            DropColumn("dbo.Documents", "CourseDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "CourseDescription", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Documents", "CourseName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Documents", "DocumentName");
        }
    }
}
