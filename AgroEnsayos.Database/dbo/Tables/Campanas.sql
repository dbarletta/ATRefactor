CREATE TABLE [dbo].[Campanas] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [CategoriaId] INT          NOT NULL,
    [Nombre]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Campanas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Campanas_Categorias] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[Categorias] ([Id])
);

