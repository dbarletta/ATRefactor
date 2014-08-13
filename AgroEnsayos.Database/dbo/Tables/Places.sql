CREATE TABLE [dbo].[Places] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Region]       VARCHAR (30) NULL,
    [Province]    VARCHAR (50) NULL,
    [Department] VARCHAR (50) NULL,
    [Header]     VARCHAR (50) NULL,
    [Locality]    VARCHAR (50) NULL,
    [Latitude]      FLOAT (53)   NULL,
    [Longitude]     FLOAT (53)   NULL,
    [Perimeter]    XML          NULL,
    CONSTRAINT [PK_Lugares] PRIMARY KEY CLUSTERED ([Id] ASC)
);

