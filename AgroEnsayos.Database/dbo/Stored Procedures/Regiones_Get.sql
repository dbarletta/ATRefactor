
--Atributos_Get 4
CREATE PROCEDURE [dbo].[Regiones_Get]
	@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT distinct region 
	From [Places] 
	where 
		Region is not null 
		AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select l.[PlaceId] from [Products] p , [ProductPlace] l where l.[ProductId]=p.Id and [CategoryId]=@categoriaId)))
	Order by Region
	
END

