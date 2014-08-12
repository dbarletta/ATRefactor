CREATE PROCEDURE [dbo].[Productos_Delete] 
@Id int
AS
BEGIN

update Productos set Deshabilitado = 1 where Id = @Id
END