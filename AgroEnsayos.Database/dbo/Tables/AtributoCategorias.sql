CREATE TABLE [dbo].[AtributoCategorias] (
    [CategoriaId] INT NOT NULL,
    [AtributoId]  INT NOT NULL,
    CONSTRAINT [PK_AtributoCategorias] PRIMARY KEY CLUSTERED ([CategoriaId] ASC, [AtributoId] ASC),
    CONSTRAINT [FK_AtributoCategorias_Atributos] FOREIGN KEY ([AtributoId]) REFERENCES [dbo].[Atributos] ([Id]),
    CONSTRAINT [FK_AtributoCategorias_Categorias] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[Categorias] ([Id])
);

