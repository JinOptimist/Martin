namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFileNameForCoverAndMp3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "CoverFileName", c => c.String());
            AddColumn("dbo.Song", "Mp3FileName", c => c.String());
            DropColumn("dbo.Album", "CoverUrl");
            DropColumn("dbo.Song", "Mp3Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Song", "Mp3Path", c => c.String());
            AddColumn("dbo.Album", "CoverUrl", c => c.String());
            DropColumn("dbo.Song", "Mp3FileName");
            DropColumn("dbo.Album", "CoverFileName");
        }
    }
}
