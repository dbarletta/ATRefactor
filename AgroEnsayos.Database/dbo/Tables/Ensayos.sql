CREATE TABLE [dbo].[Ensayos] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CampanaId]       INT           NOT NULL,
    [ProductoId]      INT           NOT NULL,
    [LugarId]         INT           NULL,
    [Rinde]           SMALLMONEY    NOT NULL,
    [Fuente]          VARCHAR (50)  NOT NULL,
    [Establecimiento] VARCHAR (50)  NULL,
    [FechaSiembra]    DATE          NULL,
    [FechaCosecha]    DATE          NULL,
    [Indice]          INT           NULL,
    [Observaciones]   VARCHAR (100) NULL,
    [Archivo]         VARCHAR (100) NULL,
    CONSTRAINT [PK_Ensayos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ensayos_Campanas] FOREIGN KEY ([CampanaId]) REFERENCES [dbo].[Campanas] ([Id]),
    CONSTRAINT [FK_Ensayos_Lugares] FOREIGN KEY ([LugarId]) REFERENCES [dbo].[Lugares] ([Id]),
    CONSTRAINT [FK_Ensayos_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);

