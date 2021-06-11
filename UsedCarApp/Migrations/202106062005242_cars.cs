namespace UsedCarApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cars : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Make = c.String(),
                        Model = c.String(),
                        Km = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cars");
        }
    }
}
