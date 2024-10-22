namespace BookingApp_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class crateBookingInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingInfoes",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        ProfileId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: false)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: false)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProfileId)
                .Index(t => t.PackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingInfoes", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.BookingInfoes", "PackageId", "dbo.Packages");
            DropForeignKey("dbo.BookingInfoes", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.BookingInfoes", new[] { "PackageId" });
            DropIndex("dbo.BookingInfoes", new[] { "ProfileId" });
            DropIndex("dbo.BookingInfoes", new[] { "ApplicationUserId" });
            DropTable("dbo.BookingInfoes");
        }
    }
}
