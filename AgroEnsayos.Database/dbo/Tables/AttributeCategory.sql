CREATE TABLE [dbo].[AttributeCategory] (
    [CategoryId] INT NOT NULL,
    [AttributeId]  INT NOT NULL,
    CONSTRAINT [PK_AttributeCategory] PRIMARY KEY CLUSTERED ([CategoryId] ASC, [AttributeId] ASC),
    CONSTRAINT [FK_AttributeCategory_Attribute] FOREIGN KEY ([AttributeId]) REFERENCES [dbo].[Attributes] ([Id]),
    CONSTRAINT [FK_AttributeCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
);

