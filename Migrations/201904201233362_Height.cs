namespace WindEnergy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Height : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WindGenerators", "Height", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WindGenerators", "Height");
        }
    }
}
