namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlgueresStateAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AluguerState",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Alugueres", "AluguerState_id", c => c.Byte(nullable: false));
            CreateIndex("dbo.Alugueres", "AluguerState_id");
         //   AddForeignKey("dbo.Alugueres", "AluguerState_id", "dbo.AluguerState", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
         //   DropForeignKey("dbo.Alugueres", "AluguerState_id", "dbo.AluguerState");
            DropIndex("dbo.Alugueres", new[] { "AluguerState_id" });
            DropColumn("dbo.Alugueres", "AluguerState_id");
            DropTable("dbo.AluguerState");
        }
    }
}
