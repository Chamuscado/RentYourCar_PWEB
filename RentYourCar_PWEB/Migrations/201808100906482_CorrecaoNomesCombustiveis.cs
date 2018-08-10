namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrecaoNomesCombustiveis : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Combustiveis SET Nome = 'Diesel' WHERE Id = 2");
            Sql("UPDATE Combustiveis SET Nome = 'Elétrico' WHERE Id = 3");
            Sql("UPDATE Combustiveis SET Nome = 'Híbrido' WHERE Id = 4");
            Sql("UPDATE Combustiveis SET Nome = 'Gás' WHERE Id = 5");
        }
        
        public override void Down()
        {
        }
    }
}
