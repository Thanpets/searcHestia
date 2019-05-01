namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultPricePerNight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VacProperties", "PricePN", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VacProperties", "PricePN");
        }
    }
}
