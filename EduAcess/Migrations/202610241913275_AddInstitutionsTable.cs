namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInstitutionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ShortName = c.String(),
                        Type = c.String(nullable: false),
                        Province = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Institutions");
        }
    }
}
