CREATE TABLE [dbo].[ProductoLugares] (
    [ProductoId] INT NOT NULL,
    [LugarId]    INT NOT NULL,
    CONSTRAINT [PK_ProductoLugares] PRIMARY KEY CLUSTERED ([ProductoId] ASC, [LugarId] ASC),
    CONSTRAINT [FK_ProductoLugares_Lugares] FOREIGN KEY ([LugarId]) REFERENCES [dbo].[Lugares] ([Id]),
    CONSTRAINT [FK_ProductoLugares_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);

