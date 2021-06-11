namespace UsedCarApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ads", "Price");
        }
    }
}
