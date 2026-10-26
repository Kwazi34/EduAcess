namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBursariesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bursaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Provider = c.String(nullable: false),
                        FieldOfStudy = c.String(),
                        OpeningDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        Description = c.String(),
                        Website = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bursaries");
        }
    }
}
