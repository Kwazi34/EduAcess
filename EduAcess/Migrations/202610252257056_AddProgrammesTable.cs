namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgrammesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Programmes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        QualificationType = c.String(nullable: false),
                        FieldOfStudy = c.String(nullable: false),
                        RequiredAps = c.Int(nullable: false),
                        DurationYears = c.Int(nullable: false),
                        RequiredSubjects = c.String(),
                        InstitutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId, cascadeDelete: true)
                .Index(t => t.InstitutionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Programmes", "InstitutionId", "dbo.Institutions");
            DropIndex("dbo.Programmes", new[] { "InstitutionId" });
            DropTable("dbo.Programmes");
        }
    }
}
