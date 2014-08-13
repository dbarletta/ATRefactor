CREATE PROCEDURE [dbo].[Atributos_GetAllWithProductValues]
	@ProductoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		  a.Id
		, isNull(a.[Family],'Propiedades') Rubro
		, a.[Name]
		, a.[DataType]
		, a.Tags
		, pa.[OriginalValue]   
	FROM [Attributes] a
		INNER JOIN [AttributeCategory] ac on ac.[AttributeId] = a.Id
		LEFT JOIN [ProductAttribute] pa on pa.[AttributeId] = a.Id and pa.[ProductId] = @ProductoId
	WHERE ac.[CategoryId] = (select [CategoryId] from [Products] where Id = @ProductoId) AND a.[IsDisabled] = 0
	ORDER BY isNull(a.[Family],'aa') , a.[Name]
END