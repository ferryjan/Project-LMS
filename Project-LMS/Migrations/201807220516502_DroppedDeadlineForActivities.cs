namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DroppedDeadlineForActivities : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Activities", "Deadline");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "Deadline", c => c.DateTime());
        }
    }
}
