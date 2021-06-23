namespace UsedCarApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class switchedkm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "Km", c => c.Int(nullable: false));
            DropColumn("dbo.Cars", "Km");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cars", "Km", c => c.Int(nullable: false));
            DropColumn("dbo.Ads", "Km");
        }
    }
}
