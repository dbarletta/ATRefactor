
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Localidades_Get]
	@categoriaId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct l.Localidad From Lugares l 
	where l.Localidad is not null 
			AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select e.LugarId from Ensayos e, Productos p where e.ProductoId=p.Id and CategoriaId=@categoriaId)))
	Order by l.Localidad
	
END

