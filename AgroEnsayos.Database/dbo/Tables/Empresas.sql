CREATE TABLE [dbo].[Empresas] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]        VARCHAR (50)  NOT NULL,
    [Deshabilitada] BIT           CONSTRAINT [DF_Empresas_Deshabilitada] DEFAULT ((0)) NOT NULL,
    [domicilio]     VARCHAR (250) NULL,
    [codigo_postal] VARCHAR (100) NULL,
    [localidad]     VARCHAR (250) NULL,
    [pais]          VARCHAR (100) NULL,
    [telefono]      VARCHAR (100) NULL,
    [fax]           VARCHAR (100) NULL,
    [email]         VARCHAR (100) NULL,
    [url_logo]      VARCHAR (250) NULL,
    CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

