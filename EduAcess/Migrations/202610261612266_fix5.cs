namespace EduAcess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FaceDescriptor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FaceDescriptor");
        }
    }
}
