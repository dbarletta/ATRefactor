--Atributos_GetEquivalencias 5
CREATE PROCEDURE [dbo].[Atributos_GetEquivalencias]
	@atributoId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ae.AtributoId, a.Nombre Atributo, ae.Valor, ae.Equivalencia, ae.Escala From AtributoEquivalencias ae
	Inner Join Atributos a on ae.AtributoId = a.Id
	Where ae.AtributoId = @atributoId and a.Deshabilitado = 0
	Order by ae.Escala DESC, ae.Valor
END
