namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RetirarPrecoMensalVeiculo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Veiculos", "PrecoMensal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Veiculos", "PrecoMensal", c => c.Single(nullable: false));
        }
    }
}
