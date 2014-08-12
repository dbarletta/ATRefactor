CREATE PROCEDURE [dbo].[Atributos_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *, 0 as valor
	From Atributos a
	Where a.Id = @Id AND a.Deshabilitado = 0
END
