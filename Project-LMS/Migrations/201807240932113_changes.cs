namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Description");
        }
    }
}
