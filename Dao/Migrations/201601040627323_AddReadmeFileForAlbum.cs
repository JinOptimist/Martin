namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReadmeFileForAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "ReadmeInArchive", c => c.String());
            AlterColumn("dbo.Album", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Album", "Description", c => c.String(nullable: false));
            DropColumn("dbo.Album", "ReadmeInArchive");
        }
    }
}
