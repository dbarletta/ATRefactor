CREATE PROCEDURE [dbo].[Productos_Delete] 
@Id int
AS
BEGIN

update [Products] set [IsDisabled] = 1 where Id = @Id
END