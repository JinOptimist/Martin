namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SongOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Song", "Order", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Song", "Order");
        }
    }
}
