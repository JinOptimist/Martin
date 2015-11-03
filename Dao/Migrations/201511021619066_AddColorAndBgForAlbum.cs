namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorAndBgForAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "BackgroundFileName", c => c.String(nullable: false));
            AddColumn("dbo.Album", "TextColor", c => c.String(nullable: false));
            AlterColumn("dbo.Album", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Album", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Album", "CoverFileName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Album", "CoverFileName", c => c.String());
            AlterColumn("dbo.Album", "Description", c => c.String());
            AlterColumn("dbo.Album", "Name", c => c.String());
            DropColumn("dbo.Album", "TextColor");
            DropColumn("dbo.Album", "BackgroundFileName");
        }
    }
}
