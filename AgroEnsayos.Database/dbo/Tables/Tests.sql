CREATE TABLE [dbo].[Tests] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [CampaignId]       INT           NOT NULL,
    [ProductId]      INT           NOT NULL,
    [PlaceId]         INT           NULL,
    [Yield]           SMALLMONEY    NOT NULL,
    [Source]          VARCHAR (50)  NOT NULL,
    [Establishment] VARCHAR (50)  NULL,
    [PlantingDate]    DATE          NULL,
    [HarvestDate]    DATE          NULL,
    [Index]          INT           NULL,
    [Observations]   VARCHAR (100) NULL,
    [File]         VARCHAR (100) NULL,
    CONSTRAINT [PK_Ensayos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ensayos_Campanas] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaigns] ([Id]),
    CONSTRAINT [FK_Ensayos_Lugares] FOREIGN KEY ([PlaceId]) REFERENCES [dbo].[Places] ([Id]),
    CONSTRAINT [FK_Ensayos_Productos] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

