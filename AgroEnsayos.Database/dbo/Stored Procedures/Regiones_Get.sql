
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Regiones_Get]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct region 
	From Lugares 
	where 
		Region is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select l.LugarId from Productos p , ProductoLugares l where l.ProductoId=p.Id and CategoriaId=@categoriaId)))
	Order by Region
	
END

