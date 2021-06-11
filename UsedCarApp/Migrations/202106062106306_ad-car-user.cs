namespace UsedCarApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adcaruser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ads", "CarId", c => c.Int(nullable: false));
            AddColumn("dbo.Ads", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ads", "CarId");
            CreateIndex("dbo.Ads", "UserId");
            AddForeignKey("dbo.Ads", "CarId", "dbo.Cars", "CarId", cascadeDelete: true);
            AddForeignKey("dbo.Ads", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ads", "UserId", "dbo.Users");
            DropForeignKey("dbo.Ads", "CarId", "dbo.Cars");
            DropIndex("dbo.Ads", new[] { "UserId" });
            DropIndex("dbo.Ads", new[] { "CarId" });
            DropColumn("dbo.Ads", "UserId");
            DropColumn("dbo.Ads", "CarId");
        }
    }
}
