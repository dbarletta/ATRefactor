CREATE TABLE [dbo].[AtributoEquivalencias] (
    [AtributoId]   INT           NOT NULL,
    [Equivalencia] VARCHAR (100) NOT NULL,
    [Valor]        VARCHAR (100) NOT NULL,
    [Escala]       TINYINT       CONSTRAINT [DF_AtributoValores_Escala] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AtributoEquivalencias] PRIMARY KEY CLUSTERED ([AtributoId] ASC, [Equivalencia] ASC, [Valor] ASC),
    CONSTRAINT [FK_AtributoValores_Atributos] FOREIGN KEY ([AtributoId]) REFERENCES [dbo].[Atributos] ([Id])
);

