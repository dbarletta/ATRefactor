CREATE TABLE [dbo].[ProductPlace] (
    [ProductId] INT NOT NULL,
    [PlaceId]    INT NOT NULL,
    CONSTRAINT [PK_ProductoLugares] PRIMARY KEY CLUSTERED ([ProductId] ASC, [PlaceId] ASC),
    CONSTRAINT [FK_ProductoLugares_Lugares] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Places] ([Id]),
    CONSTRAINT [FK_ProductoLugares_Productos] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

