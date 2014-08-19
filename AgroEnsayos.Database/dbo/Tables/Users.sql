CREATE TABLE [dbo].[Users] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CompanyId]  INT          NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Password]   VARCHAR (50) NOT NULL,
    [Role]       VARCHAR (50) NOT NULL,
    [IsDisabled] BIT          CONSTRAINT [DF_Users_IsDisabled] DEFAULT ((0)) NOT NULL,
    [Firstname] VARCHAR(50) NULL, 
    [Lastname] VARCHAR(50) NULL, 
    [Email] VARCHAR(50) NULL, 
    [Province] VARCHAR(50) NULL, 
    [Locality] VARCHAR(50) NULL, 
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Users_Empresas] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Companies] ([Id])
);

