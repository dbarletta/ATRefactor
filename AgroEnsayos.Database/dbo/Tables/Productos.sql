CREATE TABLE [dbo].[Productos] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CategoriaId]    INT           NOT NULL,
    [EmpresaId]      INT           NOT NULL,
    [Nombre]         VARCHAR (50)  NOT NULL,
    [DescripcionPG]  VARCHAR (300) NULL,
    [Material]       VARCHAR (50)  NULL,
    [EsHibrido]      BIT           NULL,
    [Ciclo]          VARCHAR (50)  NULL,
    [EsConvencional] BIT           NULL,
    [DiasFloracion]  INT           NULL,
    [DiasMadurez]    INT           NULL,
    [AlturaPlanta]   INT           NULL,
    [EsNuevo]        BIT           CONSTRAINT [DF_Productos_EsNuevo] DEFAULT ((0)) NULL,
    [Alta]           INT           NULL,
    [FechaCarga]     DATE          NULL,
    [Deshabilitado]  BIT           CONSTRAINT [DF_Productos_Deshabilitado] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[Categorias] ([Id]),
    CONSTRAINT [FK_Productos_Empresas] FOREIGN KEY ([EmpresaId]) REFERENCES [dbo].[Empresas] ([Id])
);

