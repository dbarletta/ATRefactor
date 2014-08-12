--Atributos_GetValores 5
CREATE PROCEDURE [dbo].[Atributos_GetValores]
	@atributoId INT
AS
BEGIN
	SET NOCOUNT ON;

	Select distinct pa.Valor From ProductoAtributos pa
	Left Join AtributoEquivalencias ae on pa.AtributoId = ae.AtributoId and pa.Valor = ae.Valor
	Where pa.AtributoId = @atributoId and ae.AtributoId is null
END

