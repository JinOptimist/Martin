namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddorderforAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Album", "Order", c => c.Int(nullable: false, defaultValue: -1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Album", "Order");
        }
    }
}
