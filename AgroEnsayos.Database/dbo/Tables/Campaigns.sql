CREATE TABLE [dbo].[Campaigns] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [CategoryId] INT          NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Campanas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Campanas_Categorias] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
);

