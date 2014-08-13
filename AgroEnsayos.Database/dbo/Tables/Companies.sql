CREATE TABLE [dbo].[Companies] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [IsDisabled] BIT           CONSTRAINT [DF_Empresas_Deshabilitada] DEFAULT ((0)) NOT NULL,
    [Address]     VARCHAR (250) NULL,
    [ZipCode] VARCHAR (100) NULL,
    [Locality]     VARCHAR (250) NULL,
    [Country]          VARCHAR (100) NULL,
    [Phone]      VARCHAR (100) NULL,
    [Fax]           VARCHAR (100) NULL,
    [Email]         VARCHAR (100) NULL,
    [LogoUrl]      VARCHAR (250) NULL,
    CONSTRAINT [PK_Empresas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

