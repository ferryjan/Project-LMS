namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMsgEntityChange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "isPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "isPublic");
        }
    }
}
