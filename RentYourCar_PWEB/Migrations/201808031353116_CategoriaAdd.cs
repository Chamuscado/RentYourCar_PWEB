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
            
            AddColumn("dbo.Veiculoes", "Categoria_Id", c => c.Byte());
            CreateIndex("dbo.Veiculoes", "Categoria_Id");
            AddForeignKey("dbo.Veiculoes", "Categoria_Id", "dbo.Categorias", "Id");
            DropColumn("dbo.Veiculoes", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Veiculoes", "Categoria", c => c.String(maxLength: 32));
            DropForeignKey("dbo.Veiculoes", "Categoria_Id", "dbo.Categorias");
            DropIndex("dbo.Veiculoes", new[] { "Categoria_Id" });
            DropColumn("dbo.Veiculoes", "Categoria_Id");
            DropTable("dbo.Categorias");
        }
    }
}
