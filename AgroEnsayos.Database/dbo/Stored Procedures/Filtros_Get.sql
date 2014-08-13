

--Atributos_GetValores 5
CREATE PROCEDURE [dbo].[Filtros_Get]
	@categoriaId INT
	,@filtro INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct a.Id , isnull(a.[Family],'Caracteristicas') Rubro, a.[DataType] , a.Tags , a.[Name] , ae.[MappedValue] valor , ae.[Scale]
	FROM [AttributeCategory] ac , [Attributes] a left join [AttributeMappings] ae on (a.Id = ae.[AttributeId])
	  where [CategoryId] = @categoriaId
	  and ac.[AttributeId] = a.Id
	  AND (@filtro = 0 OR (@filtro != 0 AND a.[IsFilter]=@filtro))
	order by isnull(a.[Family],'Caracteristicas') , a.[Name] , ae.[Scale] desc
END



