CREATE TABLE [dbo].[Categorias] (
    [Id]      INT          IDENTITY (1, 1) NOT NULL,
    [PadreId] INT          NULL,
    [Nombre]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED ([Id] ASC)
);

