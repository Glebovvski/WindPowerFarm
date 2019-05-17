namespace WindEnergy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class redesigndatabase : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Months");
            DropTable("dbo.Regions");
            DropTable("dbo.WindSpeeds");
        }
        
        public override void Down()
        {
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
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
