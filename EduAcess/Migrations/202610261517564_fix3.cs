namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LearnerProfiles", "Gender", c => c.String());
            AddColumn("dbo.LearnerProfiles", "Interests", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LearnerProfiles", "Interests");
            DropColumn("dbo.LearnerProfiles", "Gender");
        }
    }
}
