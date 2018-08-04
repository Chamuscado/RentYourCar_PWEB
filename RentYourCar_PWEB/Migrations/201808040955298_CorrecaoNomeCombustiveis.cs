namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecaoNomeCombustiveis : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Combustivels", newName: "Combustiveis");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Combustiveis", newName: "Combustivels");
        }
    }
}
