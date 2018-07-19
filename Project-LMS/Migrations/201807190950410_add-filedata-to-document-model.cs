namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfiledatatodocumentmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FileData", c => c.Binary());
            AlterColumn("dbo.Documents", "DocumentName", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "DocumentName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Documents", "FileData");
        }
    }
}
