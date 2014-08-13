
CREATE PROCEDURE [dbo].[Fuentes_Get]
	@categoriaId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct e.[Source] From [Tests] e 
	where e.[Source] is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND [ProductId] in (select Id from [Products] p where [CategoryId]=@categoriaId)))
	Order by e.[Source]
	
END

