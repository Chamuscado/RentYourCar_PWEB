namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriadaAvaliacaoFornecedor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliacoesFornecedores",
                c => new
                    {
                        AluguerId = c.Int(nullable: false),
                        Comentario = c.String(nullable: false, maxLength: 2048),
                        Simpatia = c.Byte(nullable: false),
                        Rapidez = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.AluguerId)
                .ForeignKey("dbo.Alugueres", t => t.AluguerId)
                .Index(t => t.AluguerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AvaliacoesFornecedores", "AluguerId", "dbo.Alugueres");
            DropIndex("dbo.AvaliacoesFornecedores", new[] { "AluguerId" });
            DropTable("dbo.AvaliacoesFornecedores");
        }
    }
}
