
CREATE PROCEDURE [dbo].[Campana_GetByCategoria]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.Id , c.[CategoryId] , c.[Name] From [Campaigns] c
	where 
		(@categoriaId = 0 OR (@categoriaId != 0 AND c.[CategoryId]=@categoriaId))
	Order by c.[Name]
	
END

