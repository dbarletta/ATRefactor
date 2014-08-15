CREATE TABLE [dbo].[UserCategory]
(
	[UserId] INT NOT NULL, 
    [CategoryId] INT NOT NULL, 
    PRIMARY KEY ([CategoryId], [UserId])
)
