namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserLogin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artist",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Password = c.String(nullable: false),
                        MegaAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);

            Sql("INSERT INTO Artist VALUES (N'Jin', N'881205', '1')");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Artist", new[] { "Name" });
            DropTable("dbo.Artist");
        }
    }
}
