namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdVeiculo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Veiculos", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Veiculos", "UserId");
        }
    }
}
