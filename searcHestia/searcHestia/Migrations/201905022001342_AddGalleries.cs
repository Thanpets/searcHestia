namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGalleries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacPropertyId = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Path = c.String(),
                        Details = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VacProperties", t => t.VacPropertyId, cascadeDelete: true)
                .Index(t => t.VacPropertyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Galleries", "VacPropertyId", "dbo.VacProperties");
            DropIndex("dbo.Galleries", new[] { "VacPropertyId" });
            DropTable("dbo.Galleries");
        }
    }
}
