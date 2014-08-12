CREATE PROCEDURE [dbo].[ProductoLugares_Delete]
@productid int
AS
BEGIN
	SET NOCOUNT ON;
delete from ProductoLugares where ProductoId = @productid

END

