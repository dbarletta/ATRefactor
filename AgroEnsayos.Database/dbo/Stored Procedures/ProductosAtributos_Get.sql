--Productos_Get 4
CREATE PROCEDURE [dbo].[ProductosAtributos_Get]
	@productoId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id ProductoId, pa.Valor , a.Id , a.Nombre , isNull(a.Rubro,'Propiedades') Rubro , a.Tags , a.TipoDato 
	From Productos p
	Inner Join ProductoAtributos pa on p.Id = pa.ProductoId
	Inner Join Atributos a on a.Id = pa.AtributoId
	Where 
		(@productoId IS NULL OR @productoId = 0 OR (@productoId IS NOT NULL AND p.Id = @productoId))
	order by isNull(a.Rubro,'aa') , a.Nombre
END
