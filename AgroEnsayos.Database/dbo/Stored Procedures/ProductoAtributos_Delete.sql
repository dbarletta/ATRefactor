CREATE PROCEDURE [dbo].[ProductoAtributos_Delete]
@ProductId int
AS
BEGIN
DELETE FROM ProductoAtributos where ProductoId = @ProductId

END