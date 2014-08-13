--Atributos_SaveEquivalencia
CREATE PROCEDURE [dbo].[Atributos_SaveEquivalencia]
	@details AtributoEquivalenciasType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	
	--Primero Borro todos las equivalencias con ese atributoid
	DELETE FROM [AttributeMappings] Where [AttributeId] In (Select TOP 1 AtributoId From @details)
	
	-- Insert statements for procedure here
	INSERT INTO [AttributeMappings] ([AttributeId], [MappedValue], [Scale], [OriginalValue])
	SELECT AtributoId, Equivalencia, Escala, Valor FROM @details
END
