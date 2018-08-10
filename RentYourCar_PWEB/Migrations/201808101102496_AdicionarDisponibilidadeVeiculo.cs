namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicionarDisponibilidadeVeiculo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculos", "InicioDisponibilidade", c => c.DateTime(nullable: false));
            AddColumn("dbo.Veiculos", "FimDisponibilidade", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Veiculos", "FimDisponibilidade");
            DropColumn("dbo.Veiculos", "InicioDisponibilidade");
        }
    }
}
