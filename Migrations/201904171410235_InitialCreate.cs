namespace WindEnergy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WindGenerators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RatedPower = c.Double(nullable: false),
                        RatedWindSpeed = c.Double(nullable: false),
                        Power = c.Double(nullable: false),
                        WindSpeed = c.Double(nullable: false),
                        MaxWindSpeed = c.Double(nullable: false),
                        MinWindSpeed = c.Double(nullable: false),
                        IsWorking = c.Boolean(nullable: false),
                        ErrorMessage = c.String(),
                        Radius = c.Double(),
                        SweptArea = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WindGenerators");
        }
    }
}
