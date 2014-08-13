CREATE TABLE [dbo].[AttributeMappings] (
    [AttributeId]   INT           NOT NULL,
    [MappedValue] VARCHAR (100) NOT NULL,
    [OriginalValue]        VARCHAR (100) NOT NULL,
    [Scale]       TINYINT       CONSTRAINT [DF_AtributoValores_Escala] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AtributoEquivalencias] PRIMARY KEY CLUSTERED ([AttributeId] ASC, [MappedValue] ASC, [OriginalValue] ASC),
    CONSTRAINT [FK_AtributoValores_Atributos] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([Id])
);

