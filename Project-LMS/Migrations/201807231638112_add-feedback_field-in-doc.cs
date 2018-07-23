namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfeedback_fieldindoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FeedBack", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "FeedBack");
        }
    }
}
