CREATE TABLE [dbo].[Products] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CategoryId]    INT           NOT NULL,
    [CompanyId]      INT           NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    [Description]  VARCHAR (300) NULL,
    [Material]       VARCHAR (50)  NULL,
    [IsHybrid]      BIT           NULL,
    [Cycle]          VARCHAR (50)  NULL,
    [IsConventional] BIT           NULL,
    [DaysToFlowering]  INT           NULL,
    [DaysToMaturity]    INT           NULL,
    [PlantHeight]   INT           NULL,
    [IsNew]        BIT           CONSTRAINT [DF_Productos_EsNuevo] DEFAULT ((0)) NULL,
    [Height]           INT           NULL,
    [EntryDate]     DATE          NULL,
    [IsDisabled]  BIT           CONSTRAINT [DF_Productos_Deshabilitado] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Productos_Empresas] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

