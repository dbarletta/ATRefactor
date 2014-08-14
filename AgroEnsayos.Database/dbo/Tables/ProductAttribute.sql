CREATE TABLE [dbo].[ProductAttribute] (
    [ProductId]          INT NOT NULL,
    [AttributeMappingId] INT NOT NULL,
    CONSTRAINT [PK_ProductoAtributos] PRIMARY KEY CLUSTERED ([ProductId] ASC, [AttributeMappingId] ASC),
    CONSTRAINT [FK_ProductAttribute_AttributeMappings] FOREIGN KEY ([AttributeMappingId]) REFERENCES [dbo].[AttributeMappings] ([AttributeMappingId]),
    CONSTRAINT [FK_ProductoAtributos_Productos] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);



