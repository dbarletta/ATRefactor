--Atributos_GetEquivalencias 5
CREATE PROCEDURE [dbo].[Atributos_GetEquivalencias]
	@atributoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ae.[AttributeId], a.[Name] Atributo, ae.[OriginalValue], ae.[MappedValue], ae.[Scale] From [AttributeMappings] ae
	Inner Join [Attributes] a on ae.[AttributeId] = a.Id
	Where ae.[AttributeId] = @atributoId and a.[IsDisabled] = 0
	Order by ae.[Scale] DESC, ae.[OriginalValue]
END
