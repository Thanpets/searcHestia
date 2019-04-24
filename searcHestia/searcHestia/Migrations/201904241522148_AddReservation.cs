namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReservation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pricings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacPropertyId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Description = c.String(),
                        OccRate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VacProperties", t => t.VacPropertyId, cascadeDelete: true)
                .Index(t => t.VacPropertyId);
            
            CreateTable(
                "dbo.RatCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RatingId = c.Int(nullable: false),
                        Title = c.String(),
                        Value = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ratings", t => t.RatingId, cascadeDelete: true)
                .Index(t => t.RatingId);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Overall = c.Single(nullable: false),
                        ReservationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reservations", t => t.ReservationId)
                .Index(t => t.ReservationId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VacPropertyId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        OccupantsNum = c.Int(nullable: false),
                        DateBooked = c.DateTime(nullable: false),
                        CustComments = c.String(),
                        PricePN = c.Double(nullable: false),
                        RStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VacProperties", t => t.VacPropertyId, cascadeDelete: true)
                .Index(t => t.VacPropertyId);
            
            AlterColumn("dbo.Locations", "LatCoordinate", c => c.Double());
            AlterColumn("dbo.Locations", "LngCoordinate", c => c.Double());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "VacPropertyId", "dbo.VacProperties");
            DropForeignKey("dbo.Ratings", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.RatCategories", "RatingId", "dbo.Ratings");
            DropForeignKey("dbo.Pricings", "VacPropertyId", "dbo.VacProperties");
            DropIndex("dbo.Reservations", new[] { "VacPropertyId" });
            DropIndex("dbo.Ratings", new[] { "ReservationId" });
            DropIndex("dbo.RatCategories", new[] { "RatingId" });
            DropIndex("dbo.Pricings", new[] { "VacPropertyId" });
            AlterColumn("dbo.Locations", "LngCoordinate", c => c.Double(nullable: false));
            AlterColumn("dbo.Locations", "LatCoordinate", c => c.Double(nullable: false));
            DropTable("dbo.Reservations");
            DropTable("dbo.Ratings");
            DropTable("dbo.RatCategories");
            DropTable("dbo.Pricings");
        }
    }
}
