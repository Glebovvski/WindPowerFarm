namespace WindEnergy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Extratablesadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WindSpeeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Speed = c.Double(nullable: false),
                        MonthId = c.Int(nullable: false),
                        RegionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WindSpeeds");
            DropTable("dbo.Regions");
            DropTable("dbo.Months");
        }
    }
}
