--Productos_Get 4
CREATE PROCEDURE [dbo].[ProductosAtributos_Get]
	@productoId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id ProductoId, pa.[OriginalValue] , a.Id , a.[Name] , isNull(a.[Family],'Propiedades') Rubro , a.Tags , a.[DataType] 
	From [Products] p
	Inner Join [ProductAttribute] pa on p.Id = pa.[ProductId]
	Inner Join [Attributes] a on a.Id = pa.[AttributeId]
	Where 
		(@productoId IS NULL OR @productoId = 0 OR (@productoId IS NOT NULL AND p.Id = @productoId))
	order by isNull(a.[Family],'aa') , a.[Name]
END
