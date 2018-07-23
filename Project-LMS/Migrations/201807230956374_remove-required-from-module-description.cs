namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removerequiredfrommoduledescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Modules", "Description", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Modules", "Description", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
