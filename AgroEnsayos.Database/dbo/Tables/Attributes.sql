CREATE TABLE [dbo].[Attributes] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Family]          VARCHAR (50)  NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [DataType]       TINYINT       CONSTRAINT [DF_Atributos_TipoDato] DEFAULT ((1)) NOT NULL,
    [Tags]           VARCHAR (200) NULL,
    [IsFilter] BIT           CONSTRAINT [DF_Atributos_UsarComoFiltro] DEFAULT ((0)) NOT NULL,
    [IsDisabled] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Atributos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

