namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedMobilePhoneNumber : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "MobileNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "MobileNumber", c => c.String());
        }
    }
}
