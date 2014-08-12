CREATE TABLE [dbo].[Atributos] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Rubro]          VARCHAR (50)  NULL,
    [Nombre]         VARCHAR (50)  NOT NULL,
    [TipoDato]       TINYINT       CONSTRAINT [DF_Atributos_TipoDato] DEFAULT ((1)) NOT NULL,
    [Tags]           VARCHAR (200) NULL,
    [UsarComoFiltro] BIT           CONSTRAINT [DF_Atributos_UsarComoFiltro] DEFAULT ((0)) NOT NULL,
    [Deshabilitado] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Atributos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

