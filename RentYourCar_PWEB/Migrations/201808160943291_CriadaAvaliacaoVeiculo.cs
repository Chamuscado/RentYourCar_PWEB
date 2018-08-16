namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriadaAvaliacaoVeiculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliacoesVeiculos",
                c => new
                    {
                        AluguerId = c.Int(nullable: false),
                        Comentario = c.String(nullable: false, maxLength: 2048),
                        Limpeza = c.Byte(nullable: false),
                        Consumo = c.Byte(nullable: false),
                        Apresentacao = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AluguerId)
                .ForeignKey("dbo.Alugueres", t => t.AluguerId)
                .Index(t => t.AluguerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvaliacoesVeiculos", "AluguerId", "dbo.Alugueres");
            DropIndex("dbo.AvaliacoesVeiculos", new[] { "AluguerId" });
            DropTable("dbo.AvaliacoesVeiculos");
        }
    }
}
