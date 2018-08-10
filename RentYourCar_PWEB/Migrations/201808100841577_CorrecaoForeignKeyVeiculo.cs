namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecaoForeignKeyVeiculo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Veiculos", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Veiculos", "UserId");
            RenameColumn(table: "dbo.Veiculos", name: "ApplicationUser_Id", newName: "UserId");
            AlterColumn("dbo.Veiculos", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Veiculos", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Veiculos", new[] { "UserId" });
            AlterColumn("dbo.Veiculos", "UserId", c => c.String());
            RenameColumn(table: "dbo.Veiculos", name: "UserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Veiculos", "UserId", c => c.String());
            CreateIndex("dbo.Veiculos", "ApplicationUser_Id");
        }
    }
}
