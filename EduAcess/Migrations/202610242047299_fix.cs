namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LearnerProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        HouseholdIncome = c.String(),
                        APS = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LearnerSubjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LearnerProfileId = c.Int(nullable: false),
                        SubjectName = c.String(nullable: false),
                        Mark = c.Int(nullable: false),
                        IsLifeOrientation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LearnerProfiles", t => t.LearnerProfileId, cascadeDelete: true)
                .Index(t => t.LearnerProfileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LearnerSubjects", "LearnerProfileId", "dbo.LearnerProfiles");
            DropIndex("dbo.LearnerSubjects", new[] { "LearnerProfileId" });
            DropTable("dbo.LearnerSubjects");
            DropTable("dbo.LearnerProfiles");
        }
    }
}
