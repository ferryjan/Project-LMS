namespace Project_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedocumentdatatype : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "DocumentTypeId", "dbo.DocumentTypes");
            DropIndex("dbo.Documents", new[] { "DocumentTypeId" });
            DropColumn("dbo.Documents", "DocumentTypeId");
            DropTable("dbo.DocumentTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.DocumentTypeId);
            
            AddColumn("dbo.Documents", "DocumentTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "DocumentTypeId");
            AddForeignKey("dbo.Documents", "DocumentTypeId", "dbo.DocumentTypes", "DocumentTypeId", cascadeDelete: true);
        }
    }
}
