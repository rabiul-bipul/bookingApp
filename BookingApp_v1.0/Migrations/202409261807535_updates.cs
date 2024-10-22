namespace BookingApp_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hotels",
                c => new
                {
                    HotelId = c.Int(nullable: false, identity: true),
                    HotelName = c.String(nullable: false, maxLength: 100),
                    HotelWebsite = c.String(maxLength: 255),
                    LocationId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.HotelId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .Index(t => t.LocationId);
        }
        
        public override void Down()
        {
        }
    }
}
