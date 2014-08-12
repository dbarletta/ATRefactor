
CREATE PROCEDURE [dbo].[Campana_Get]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.Id , c.CategoriaId , c.Nombre From Campanas c
	where 
		(@Id = 0 OR (@Id != 0 AND c.Id=@Id))
	Order by c.Nombre
	
END

