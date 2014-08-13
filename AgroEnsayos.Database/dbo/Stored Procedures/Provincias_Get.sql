
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Provincias_Get]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct l.[Province] From [Places] l 
	where 
		l.[Province] is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select e.[PlaceId] from [Products] p , [Tests] e where e.[ProductId]=p.Id and [CategoryId]=@categoriaId)))
	Order by l.[Province]
	
END

