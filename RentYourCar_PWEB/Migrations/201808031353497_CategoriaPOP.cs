namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriaPOP : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[Categorias] ([Id], [Nome]) VALUES (1, N'Ligeiro Passageiros')
            INSERT INTO[dbo].[Categorias]([Id], [Nome]) VALUES(2, N'Ligeiro Mecadorias')
            INSERT INTO[dbo].[Categorias]([Id], [Nome]) VALUES(3, N'Pesado Passagerios')
            INSERT INTO[dbo].[Categorias]([Id], [Nome]) VALUES(4, N'Pesado Mecadorias')");
        }
        
        public override void Down()
        {
        }
    }
}
