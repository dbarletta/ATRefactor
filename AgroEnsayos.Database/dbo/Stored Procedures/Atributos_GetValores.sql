--Atributos_GetValores 5
CREATE PROCEDURE [dbo].[Atributos_GetValores]
	@atributoId INT
AS
BEGIN
	SET NOCOUNT ON;

	Select distinct pa.[OriginalValue] From [ProductAttribute] pa
	Left Join [AttributeMappings] ae on pa.[AttributeId] = ae.[AttributeId] and pa.[OriginalValue] = ae.[OriginalValue]
	Where pa.[AttributeId] = @atributoId and ae.[AttributeId] is null
END

