CREATE PROCEDURE [dbo].[Atributos_Get]
	@CategoriaId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT a.Id, a.Rubro, a.Nombre, a.TipoDato, a.Tags , a.UsarComoFiltro, 0 as valor, cat.Id as CategoriaId, cat.Nombre as Categoria 
	From Atributos a
	INNER JOIN AtributoCategorias ac on ac.AtributoId = Id
	INNER JOIN Categorias cat on cat.Id = ac.CategoriaId
	Where 
		a.Deshabilitado = 0
		AND(@CategoriaId IS NULL 
		    OR @CategoriaId = 0 
		    OR (@CategoriaId IS NOT NULL 
			    AND a.Id IN (Select AtributoId From AtributoCategorias Where CategoriaId = @CategoriaId)))
	Order by a.Nombre
END