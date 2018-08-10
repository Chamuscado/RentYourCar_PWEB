namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarRegiaoVeiculo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculos", "Regiao", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Veiculos", "Regiao");
        }
    }
}
