CREATE TABLE [dbo].[Categories] (
    [Id]      INT          IDENTITY (1, 1) NOT NULL,
    [ParentId] INT          NULL,
    [Name]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED ([Id] ASC)
);

