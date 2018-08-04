namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriaAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Veiculos", "Categoria_Id", c => c.Byte());
            CreateIndex("dbo.Veiculos", "Categoria_Id");
            AddForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias", "Id");
            DropColumn("dbo.Veiculos", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Veiculos", "Categoria", c => c.String(maxLength: 32));
            DropForeignKey("dbo.Veiculos", "Categoria_Id", "dbo.Categorias");
            DropIndex("dbo.Veiculos", new[] { "Categoria_Id" });
            DropColumn("dbo.Veiculos", "Categoria_Id");
            DropTable("dbo.Categorias");
        }
    }
}
