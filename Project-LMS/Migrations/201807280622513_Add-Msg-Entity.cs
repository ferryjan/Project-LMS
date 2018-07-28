namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMsgEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SentFrom = c.String(),
                        SentTo = c.String(),
                        SentDate = c.DateTime(nullable: false),
                        isRead = c.Boolean(nullable: false),
                        Topic = c.String(nullable: false, maxLength: 100),
                        Msg = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
