
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Localidades_Get]
	@categoriaId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct l.[Locality] From [Places] l 
	where l.[Locality] is not null 
			AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select e.[PlaceId] from [Tests] e, [Products] p where e.[ProductId]=p.Id and [CategoryId]=@categoriaId)))
	Order by l.[Locality]
	
END

