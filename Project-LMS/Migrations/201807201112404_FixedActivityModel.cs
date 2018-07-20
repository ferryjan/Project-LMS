namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedActivityModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "ActivityName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Activities", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Activities", "End", c => c.DateTime(nullable: false));
            AddColumn("dbo.Activities", "Description", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Activities", "CourseName");
            DropColumn("dbo.Activities", "StartDate");
            DropColumn("dbo.Activities", "EndDate");
            DropColumn("dbo.Activities", "ActivityDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "ActivityDescription", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Activities", "EndDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Activities", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Activities", "CourseName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Activities", "Description");
            DropColumn("dbo.Activities", "End");
            DropColumn("dbo.Activities", "Start");
            DropColumn("dbo.Activities", "ActivityName");
        }
    }
}
