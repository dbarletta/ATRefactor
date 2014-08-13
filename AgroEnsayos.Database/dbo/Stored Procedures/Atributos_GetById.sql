CREATE PROCEDURE [dbo].[Atributos_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *, 0 as valor
	From [Attributes] a
	Where a.Id = @Id AND a.[IsDisabled] = 0
END
