

--Atributos_GetValores 5
CREATE PROCEDURE [dbo].[Filtros_Get]
	@categoriaId INT
	,@filtro INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct a.Id , isnull(a.Rubro,'Caracteristicas') Rubro, a.TipoDato , a.Tags , a.Nombre , ae.Equivalencia valor , ae.Escala
	FROM AtributoCategorias ac , Atributos a left join AtributoEquivalencias ae on (a.Id = ae.AtributoId)
	  where CategoriaId = @categoriaId
	  and ac.AtributoId = a.Id
	  AND (@filtro = 0 OR (@filtro != 0 AND a.UsarComoFiltro=@filtro))
	order by isnull(a.Rubro,'Caracteristicas') , a.Nombre , ae.Escala desc
END



