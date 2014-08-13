CREATE PROCEDURE [dbo].[ProductoAtributos_Delete]
@ProductId int
AS
BEGIN
DELETE FROM [ProductAttribute] where [ProductId] = @ProductId

END