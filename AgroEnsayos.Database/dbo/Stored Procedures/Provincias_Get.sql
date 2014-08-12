
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Provincias_Get]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct l.Provincia From Lugares l 
	where 
		l.Provincia is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select e.LugarId from Productos p , Ensayos e where e.ProductoId=p.Id and CategoriaId=@categoriaId)))
	Order by l.Provincia
	
END

