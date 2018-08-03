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
            
            AddColumn("dbo.Veiculoes", "Combustivel_Id", c => c.Byte());
            CreateIndex("dbo.Veiculoes", "Combustivel_Id");
            AddForeignKey("dbo.Veiculoes", "Combustivel_Id", "dbo.Combustivels", "Id");
            DropColumn("dbo.Veiculoes", "Combustivel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Veiculoes", "Combustivel", c => c.Int(nullable: false));
            DropForeignKey("dbo.Veiculoes", "Combustivel_Id", "dbo.Combustivels");
            DropIndex("dbo.Veiculoes", new[] { "Combustivel_Id" });
            DropColumn("dbo.Veiculoes", "Combustivel_Id");
            DropTable("dbo.Combustivels");
        }
    }
}
