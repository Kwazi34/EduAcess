namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteVisits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SiteVisits");
        }
    }
}
