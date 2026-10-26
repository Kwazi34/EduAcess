namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgeGroup : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LearnerProfiles", "AgeGroup", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LearnerProfiles", "AgeGroup");
        }
    }
}
