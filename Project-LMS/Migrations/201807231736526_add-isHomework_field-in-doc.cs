namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addisHomework_fieldindoc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "isHomework", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "isHomework");
        }
    }
}
