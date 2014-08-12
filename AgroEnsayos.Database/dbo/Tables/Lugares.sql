CREATE TABLE [dbo].[Lugares] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Region]       VARCHAR (30) NULL,
    [Provincia]    VARCHAR (50) NULL,
    [Departamento] VARCHAR (50) NULL,
    [Cabecera]     VARCHAR (50) NULL,
    [Localidad]    VARCHAR (50) NULL,
    [Latitud]      FLOAT (53)   NULL,
    [Longitud]     FLOAT (53)   NULL,
    [Perimetro]    XML          NULL,
    CONSTRAINT [PK_Lugares] PRIMARY KEY CLUSTERED ([Id] ASC)
);

