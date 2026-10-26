namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLanguageAndProvince : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LearnerProfiles", "Language", c => c.String());
            AddColumn("dbo.LearnerProfiles", "Province", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LearnerProfiles", "Province");
            DropColumn("dbo.LearnerProfiles", "Language");
        }
    }
}
