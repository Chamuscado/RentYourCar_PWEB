namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VeiculosObjToBytes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias");
            DropForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis");
            DropIndex("dbo.Veiculos", new[] { "Categoria_Id" });
            DropIndex("dbo.Veiculos", new[] { "Combustivel_Id" });
            AlterColumn("dbo.Veiculos", "Categoria_id", c => c.Byte(nullable: false));
            AlterColumn("dbo.Veiculos", "Combustivel_id", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Veiculos", "Combustivel_id", c => c.Byte());
            AlterColumn("dbo.Veiculos", "Categoria_id", c => c.Byte());
            CreateIndex("dbo.Veiculos", "Combustivel_Id");
            CreateIndex("dbo.Veiculos", "Categoria_Id");
            AddForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustiveis", "Id");
            AddForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias", "Id");
        }
    }
}
