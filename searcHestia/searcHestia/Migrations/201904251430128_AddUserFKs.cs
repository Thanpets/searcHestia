namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserFKs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VacProperties", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Reservations", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.VacProperties", "UserId");
            CreateIndex("dbo.Reservations", "UserId");
            AddForeignKey("dbo.Reservations", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.VacProperties", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VacProperties", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.VacProperties", new[] { "UserId" });
            DropColumn("dbo.Reservations", "UserId");
            DropColumn("dbo.VacProperties", "UserId");
        }
    }
}
