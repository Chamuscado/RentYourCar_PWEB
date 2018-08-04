namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MudaNomeTabelVeiculos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Veiculos", newName: "Veiculos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Veiculos", newName: "Veiculos");
        }
    }
}
