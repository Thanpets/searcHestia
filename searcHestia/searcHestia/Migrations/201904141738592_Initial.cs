namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Amentities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VacProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        LocationId = c.Int(nullable: false),
                        MaxOccupancy = c.Int(nullable: false),
                        VPType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionId = c.Int(nullable: false),
                        Address = c.String(maxLength: 50),
                        ZIPCode = c.String(maxLength: 6),
                        LatCoordinate = c.Double(nullable: false),
                        LngCoordinate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.RegionId, cascadeDelete: true)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Availabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacPropertyId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VacProperties", t => t.VacPropertyId, cascadeDelete: true)
                .Index(t => t.VacPropertyId);
            
            CreateTable(
                "dbo.PropertyAmentity",
                c => new
                    {
                        VacPropertyId = c.Int(nullable: false),
                        AmentityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VacPropertyId, t.AmentityId })
                .ForeignKey("dbo.VacProperties", t => t.VacPropertyId, cascadeDelete: true)
                .ForeignKey("dbo.Amentities", t => t.AmentityId, cascadeDelete: true)
                .Index(t => t.VacPropertyId)
                .Index(t => t.AmentityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Availabilities", "VacPropertyId", "dbo.VacProperties");
            DropForeignKey("dbo.VacProperties", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "RegionId", "dbo.Regions");
            DropForeignKey("dbo.PropertyAmentity", "AmentityId", "dbo.Amentities");
            DropForeignKey("dbo.PropertyAmentity", "VacPropertyId", "dbo.VacProperties");
            DropIndex("dbo.PropertyAmentity", new[] { "AmentityId" });
            DropIndex("dbo.PropertyAmentity", new[] { "VacPropertyId" });
            DropIndex("dbo.Availabilities", new[] { "VacPropertyId" });
            DropIndex("dbo.Locations", new[] { "RegionId" });
            DropIndex("dbo.VacProperties", new[] { "LocationId" });
            DropTable("dbo.PropertyAmentity");
            DropTable("dbo.Availabilities");
            DropTable("dbo.Regions");
            DropTable("dbo.Locations");
            DropTable("dbo.VacProperties");
            DropTable("dbo.Amentities");
        }
    }
}
