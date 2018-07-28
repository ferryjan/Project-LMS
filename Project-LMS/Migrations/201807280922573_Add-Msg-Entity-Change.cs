namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMsgEntityChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageBoxNumber", c => c.String());
            AddColumn("dbo.Messages", "FirstPersonLeft", c => c.String());
            AddColumn("dbo.Messages", "SecondPersonLeft", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "SecondPersonLeft");
            DropColumn("dbo.Messages", "FirstPersonLeft");
            DropColumn("dbo.Messages", "MessageBoxNumber");
        }
    }
}
