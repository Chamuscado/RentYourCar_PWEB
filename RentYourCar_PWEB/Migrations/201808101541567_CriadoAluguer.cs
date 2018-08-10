namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriadoAluguer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alugueres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClienteId = c.String(nullable: false, maxLength: 128),
                        VeiculoId = c.Int(nullable: false),
                        Inicio = c.DateTime(nullable: false),
                        Fim = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ClienteId, cascadeDelete: true)
                .ForeignKey("dbo.Veiculos", t => t.VeiculoId, cascadeDelete: true)
                .Index(t => t.ClienteId)
                .Index(t => t.VeiculoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alugueres", "VeiculoId", "dbo.Veiculos");
            DropForeignKey("dbo.Alugueres", "ClienteId", "dbo.AspNetUsers");
            DropIndex("dbo.Alugueres", new[] { "VeiculoId" });
            DropIndex("dbo.Alugueres", new[] { "ClienteId" });
            DropTable("dbo.Alugueres");
        }
    }
}
