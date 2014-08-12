CREATE PROCEDURE [dbo].[ProductoAtributos_Insert]
@ProductoId int,
@AtributoId int,
@Valor varchar(100)
AS
BEGIN
INSERT INTO ProductoAtributos VALUES(@ProductoId, @AtributoId, @Valor)

END