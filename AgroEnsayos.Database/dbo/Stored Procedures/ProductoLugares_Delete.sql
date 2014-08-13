CREATE PROCEDURE [dbo].[ProductoLugares_Delete]
@productid int
AS
BEGIN
	SET NOCOUNT ON;
delete from [ProductPlace] where [ProductId] = @productid

END

