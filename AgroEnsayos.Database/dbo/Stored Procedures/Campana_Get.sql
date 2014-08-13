
CREATE PROCEDURE [dbo].[Campana_Get]
	@Id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.Id , c.[CategoryId] , c.[Name] From [Campaigns] c
	where 
		(@Id = 0 OR (@Id != 0 AND c.Id=@Id))
	Order by c.[Name]
	
END

