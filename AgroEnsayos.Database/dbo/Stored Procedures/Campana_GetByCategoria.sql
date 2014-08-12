
CREATE PROCEDURE [dbo].[Campana_GetByCategoria]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.Id , c.CategoriaId , c.Nombre From Campanas c
	where 
		(@categoriaId = 0 OR (@categoriaId != 0 AND c.CategoriaId=@categoriaId))
	Order by c.Nombre
	
END

