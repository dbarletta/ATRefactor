CREATE PROCEDURE [dbo].[Atributos_GetAllWithProductValues]
	@ProductoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		  a.Id
		, isNull(a.Rubro,'Propiedades') Rubro
		, a.Nombre
		, a.TipoDato
		, a.Tags
		, pa.Valor   
	FROM Atributos a
		INNER JOIN AtributoCategorias ac on ac.AtributoId = a.Id
		LEFT JOIN ProductoAtributos pa on pa.AtributoId = a.Id and pa.ProductoId = @ProductoId
	WHERE ac.CategoriaId = (select CategoriaId from Productos where Id = @ProductoId) AND a.Deshabilitado = 0
	ORDER BY isNull(a.Rubro,'aa') , a.Nombre
END