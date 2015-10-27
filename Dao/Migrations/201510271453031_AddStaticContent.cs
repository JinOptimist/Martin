namespace Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStaticContent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StaticContent",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Body = c.String(),
                        TypeStaticContent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StaticContent");
        }
    }
}
