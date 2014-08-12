CREATE TABLE [dbo].[ProductoAtributos] (
    [ProductoId] INT           NOT NULL,
    [AtributoId] INT           NOT NULL,
    [Valor]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ProductoAtributos] PRIMARY KEY CLUSTERED ([ProductoId] ASC, [AtributoId] ASC),
    CONSTRAINT [FK_ProductoAtributos_Atributos] FOREIGN KEY ([AtributoId]) REFERENCES [dbo].[Atributos] ([Id]),
    CONSTRAINT [FK_ProductoAtributos_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);

