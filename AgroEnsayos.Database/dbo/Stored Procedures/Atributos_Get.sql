CREATE PROCEDURE [dbo].[Atributos_Get]
	@CategoriaId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT a.Id, a.[Family], a.[Name], a.[DataType], a.Tags , a.[IsFilter], 0 as valor, cat.Id as CategoriaId, cat.[Name] as Categoria 
	From [Attributes] a
	INNER JOIN [AttributeCategory] ac on ac.[AttributeId] = Id
	INNER JOIN [Categories] cat on cat.Id = ac.[CategoryId]
	Where 
		a.[IsDisabled] = 0
		AND(@CategoriaId IS NULL 
		    OR @CategoriaId = 0 
		    OR (@CategoriaId IS NOT NULL 
			    AND a.Id IN (Select [AttributeId] From [AttributeCategory] Where [CategoryId] = @CategoriaId)))
	Order by a.[Name]
END