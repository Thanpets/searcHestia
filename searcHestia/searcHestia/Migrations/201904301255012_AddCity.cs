namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Amentities", newName: "Amenities");
            RenameTable(name: "dbo.PropertyAmentity", newName: "PropertyAmenity");
            DropForeignKey("dbo.Locations", "RegionId", "dbo.Regions");
            DropIndex("dbo.Locations", new[] { "RegionId" });
            RenameColumn(table: "dbo.PropertyAmenity", name: "AmentityId", newName: "AmenityId");
            RenameIndex(table: "dbo.PropertyAmenity", name: "IX_AmentityId", newName: "IX_AmenityId");
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            AddColumn("dbo.Locations", "CityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Locations", "CityId");
            AddForeignKey("dbo.Locations", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
            DropColumn("dbo.Locations", "RegionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "RegionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Locations", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "RegionId", "dbo.Regions");
            DropIndex("dbo.Cities", new[] { "RegionId" });
            DropIndex("dbo.Locations", new[] { "CityId" });
            DropColumn("dbo.Locations", "CityId");
            DropTable("dbo.Cities");
            RenameIndex(table: "dbo.PropertyAmenity", name: "IX_AmenityId", newName: "IX_AmentityId");
            RenameColumn(table: "dbo.PropertyAmenity", name: "AmenityId", newName: "AmentityId");
            CreateIndex("dbo.Locations", "RegionId");
            AddForeignKey("dbo.Locations", "RegionId", "dbo.Regions", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.PropertyAmenity", newName: "PropertyAmentity");
            RenameTable(name: "dbo.Amenities", newName: "Amentities");
        }
    }
}
