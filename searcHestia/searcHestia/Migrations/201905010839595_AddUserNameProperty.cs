namespace searcHestia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            DropColumn("dbo.AspNetUsers", "MyExtraProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "MyExtraProperty", c => c.String());
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
