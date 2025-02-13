namespace JanuaryProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smth1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Artists", "LastName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Artists", "UserId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Artists", "UserId", c => c.String());
            AlterColumn("dbo.Artists", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Artists", "Name", c => c.String(nullable: false));
        }
    }
}
