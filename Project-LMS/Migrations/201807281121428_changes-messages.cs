namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesmessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "SentFromFullName", c => c.String());
            AddColumn("dbo.Messages", "SentToFullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "SentToFullName");
            DropColumn("dbo.Messages", "SentFromFullName");
        }
    }
}
