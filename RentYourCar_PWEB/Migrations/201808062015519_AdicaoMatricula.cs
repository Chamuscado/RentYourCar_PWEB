namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdicaoMatricula : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculos", "Matricula", c => c.String(maxLength: 8));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Veiculos", "Matricula");
        }
    }
}
