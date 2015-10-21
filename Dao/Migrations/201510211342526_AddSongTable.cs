namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSongTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Song",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Lyrics = c.String(),
                        Mp3Path = c.String(),
                        Album_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Album", t => t.Album_Id, cascadeDelete: true)
                .Index(t => t.Album_Id);
            
            AddColumn("dbo.Album", "Description", c => c.String());
            AddColumn("dbo.Album", "CoverUrl", c => c.String());
            DropColumn("dbo.Album", "ImgUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Album", "ImgUrl", c => c.String());
            DropForeignKey("dbo.Song", "Album_Id", "dbo.Album");
            DropIndex("dbo.Song", new[] { "Album_Id" });
            DropColumn("dbo.Album", "CoverUrl");
            DropColumn("dbo.Album", "Description");
            DropTable("dbo.Song");
        }
    }
}
