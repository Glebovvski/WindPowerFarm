namespace WindEnergy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WindGenerators", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WindGenerators", "Price");
        }
    }
}
