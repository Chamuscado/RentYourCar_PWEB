namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecaoFKsVeiculo : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Veiculos", "Combustivel_id");
            CreateIndex("dbo.Veiculos", "Categoria_id");
            AddForeignKey("dbo.Veiculos", "Categoria_id", "dbo.Categorias", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Veiculos", "Combustivel_id", "dbo.Combustiveis", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Veiculos", "Combustivel_id", "dbo.Combustiveis");
            DropForeignKey("dbo.Veiculos", "Categoria_id", "dbo.Categorias");
            DropIndex("dbo.Veiculos", new[] { "Categoria_id" });
            DropIndex("dbo.Veiculos", new[] { "Combustivel_id" });
        }
    }
}
