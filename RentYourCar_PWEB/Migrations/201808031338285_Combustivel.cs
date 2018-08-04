namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Combustivel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Combustivels",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Veiculos", "Combustivel_Id", c => c.Byte());
            CreateIndex("dbo.Veiculos", "Combustivel_Id");
            AddForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustivels", "Id");
            DropColumn("dbo.Veiculos", "Combustivel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Veiculos", "Combustivel", c => c.Int(nullable: false));
            DropForeignKey("dbo.Veiculos", "Combustivel_Id", "dbo.Combustivels");
            DropIndex("dbo.Veiculos", new[] { "Combustivel_Id" });
            DropColumn("dbo.Veiculos", "Combustivel_Id");
            DropTable("dbo.Combustivels");
        }
    }
}
