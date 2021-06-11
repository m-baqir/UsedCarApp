namespace UsedCarApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllUsers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        AdId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Images = c.String(),
                    })
                .PrimaryKey(t => t.AdId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Phone = c.Int(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Ads");
        }
    }
}
