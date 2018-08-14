namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Classificaçoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alugueres", "ClassificacaoVeiculo_Pontos", c => c.Byte(nullable: false));
            AddColumn("dbo.Alugueres", "ClassificacaoVeiculo_Comentario", c => c.String());
            AddColumn("dbo.Alugueres", "ClassificacaoCliente_Pontos", c => c.Byte(nullable: false));
            AddColumn("dbo.Alugueres", "ClassificacaoCliente_Comentario", c => c.String());
            AddColumn("dbo.Alugueres", "ClassificacaoDono_Pontos", c => c.Byte(nullable: false));
            AddColumn("dbo.Alugueres", "ClassificacaoDono_Comentario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alugueres", "ClassificacaoDono_Comentario");
            DropColumn("dbo.Alugueres", "ClassificacaoDono_Pontos");
            DropColumn("dbo.Alugueres", "ClassificacaoCliente_Comentario");
            DropColumn("dbo.Alugueres", "ClassificacaoCliente_Pontos");
            DropColumn("dbo.Alugueres", "ClassificacaoVeiculo_Comentario");
            DropColumn("dbo.Alugueres", "ClassificacaoVeiculo_Pontos");
        }
    }
}
