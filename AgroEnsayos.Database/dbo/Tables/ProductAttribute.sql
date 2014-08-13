CREATE TABLE [dbo].[ProductAttribute] (
    [ProductId] INT           NOT NULL,
    [AttributeId] INT           NOT NULL,
    [OriginalValue]      VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ProductoAtributos] PRIMARY KEY CLUSTERED ([ProductId] ASC, [AttributeId] ASC),
    CONSTRAINT [FK_ProductoAtributos_Atributos] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([Id]),
    CONSTRAINT [FK_ProductoAtributos_Productos] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

