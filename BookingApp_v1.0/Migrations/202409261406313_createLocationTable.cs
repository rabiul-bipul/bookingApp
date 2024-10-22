namespace BookingApp_v1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createLocationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
