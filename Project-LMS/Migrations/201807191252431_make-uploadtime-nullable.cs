namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeuploadtimenullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "UploadingTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "UploadingTime", c => c.DateTime(nullable: false));
        }
    }
}
