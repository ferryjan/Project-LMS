namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somechangesindocumentmodelagain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "DocumentFileType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "DocumentFileType");
        }
    }
}
