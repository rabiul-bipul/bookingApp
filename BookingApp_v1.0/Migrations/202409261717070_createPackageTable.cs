namespace BookingApp_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createPackageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageId = c.Int(nullable: false, identity: true),
                        PackageName = c.String(nullable: false, maxLength: 100),
                        Details = c.String(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        Price = c.Double(nullable: false),
                        Sold = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                        HotelId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PackageId)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .Index(t => t.HotelId)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Packages", "HotelId", "dbo.Hotels");
            DropIndex("dbo.Packages", new[] { "LocationId" });
            DropIndex("dbo.Packages", new[] { "HotelId" });
            DropTable("dbo.Packages");
        }
    }
}
