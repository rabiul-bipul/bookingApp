namespace BookingApp_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        ProfileId = c.Int(nullable: false),
                        PackageId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        PaymentMethod = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Packages", t => t.PackageId, cascadeDelete: false)
                .ForeignKey("dbo.Profiles", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId)
                .Index(t => t.PackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoices", "ProfileId", "dbo.Profiles");
            DropForeignKey("dbo.Invoices", "PackageId", "dbo.Packages");
            DropIndex("dbo.Invoices", new[] { "PackageId" });
            DropIndex("dbo.Invoices", new[] { "ProfileId" });
            DropTable("dbo.Invoices");
        }
    }
}
