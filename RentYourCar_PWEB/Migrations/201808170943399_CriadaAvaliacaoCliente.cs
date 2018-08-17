namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriadaAvaliacaoCliente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliacoesClientes",
                c => new
                    {
                        AluguerId = c.Int(nullable: false),
                        Comentario = c.String(nullable: false, maxLength: 2048),
                        Limpeza = c.Byte(nullable: false),
                        Cuidado = c.Byte(nullable: false),
                        Pontualidade = c.Byte(nullable: false),
                        Pagamento = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AluguerId)
                .ForeignKey("dbo.Alugueres", t => t.AluguerId)
                .Index(t => t.AluguerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvaliacoesClientes", "AluguerId", "dbo.Alugueres");
            DropIndex("dbo.AvaliacoesClientes", new[] { "AluguerId" });
            DropTable("dbo.AvaliacoesClientes");
        }
    }
}
