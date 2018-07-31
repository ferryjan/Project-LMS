namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smallchangeactivity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Activities", "Description", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
