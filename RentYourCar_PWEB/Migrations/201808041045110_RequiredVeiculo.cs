namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredVeiculo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias");
            DropForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis");
            DropIndex("dbo.Veiculos", new[] { "Categoria_Id" });
            DropIndex("dbo.Veiculos", new[] { "Combustivel_Id" });
            AlterColumn("dbo.Veiculos", "Modelo", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Veiculos", "Marca", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Veiculos", "Categoria_Id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Veiculos", "Combustivel_Id", c => c.Byte(nullable: false));
            CreateIndex("dbo.Veiculos", "Categoria_Id");
            CreateIndex("dbo.Veiculos", "Combustivel_Id");
            AddForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis");
            DropForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias");
            DropIndex("dbo.Veiculos", new[] { "Combustivel_Id" });
            DropIndex("dbo.Veiculos", new[] { "Categoria_Id" });
            AlterColumn("dbo.Veiculos", "Combustivel_Id", c => c.Byte());
            AlterColumn("dbo.Veiculos", "Categoria_Id", c => c.Byte());
            AlterColumn("dbo.Veiculos", "Marca", c => c.String(maxLength: 32));
            AlterColumn("dbo.Veiculos", "Modelo", c => c.String(maxLength: 32));
            CreateIndex("dbo.Veiculos", "Combustivel_Id");
            CreateIndex("dbo.Veiculos", "Categoria_Id");
            AddForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis", "Id");
            AddForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias", "Id");
        }
    }
}
