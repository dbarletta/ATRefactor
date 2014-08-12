
CREATE PROCEDURE [dbo].[Fuentes_Get]
	@categoriaId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct e.Fuente From Ensayos e 
	where e.Fuente is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND ProductoId in (select Id from Productos p where CategoriaId=@categoriaId)))
	Order by e.Fuente
	
END

