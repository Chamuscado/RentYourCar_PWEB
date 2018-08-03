namespace RentYourCar_PWEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CombutiveisPOP : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[Combustivels] ([Id], [Nome]) VALUES (1, N'Gasolina')
            INSERT INTO[dbo].[Combustivels]([Id], [Nome]) VALUES(2, N'Disel')
            INSERT INTO[dbo].[Combustivels]([Id], [Nome]) VALUES(3, N'Eletrico')
            INSERT INTO[dbo].[Combustivels]([Id], [Nome]) VALUES(4, N'Hibrido')
            INSERT INTO[dbo].[Combustivels]([Id], [Nome]) VALUES(5, N'Gas')");
        }
        
        public override void Down()
        {
        }
    }
}
