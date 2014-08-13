CREATE TABLE [dbo].[Users] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CompanyId]  INT          NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Password]   VARCHAR (50) NOT NULL,
    [Role]       VARCHAR (50) NOT NULL,
    [IsDisabled] BIT          CONSTRAINT [DF_Users_IsDisabled] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_Empresas] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

