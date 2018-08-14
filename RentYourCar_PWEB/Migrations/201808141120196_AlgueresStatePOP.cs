namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlgueresStatePOP : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AluguerState] ([Id], [Nome]) VALUES (1, N'Aprovado')
                  INSERT INTO [dbo].[AluguerState] ([Id], [Nome]) VALUES (2, N'A decorrer')
                  INSERT INTO [dbo].[AluguerState] ([Id], [Nome]) VALUES (3, N'Aguardar aprovação')
                  INSERT INTO [dbo].[AluguerState] ([Id], [Nome]) VALUES (4, N'Rejeitado')");
        }
        
        public override void Down()
        {
        }
    }
}
