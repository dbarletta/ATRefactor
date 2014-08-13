
--Ensayos_Get 4, null, 1, 2439, 'INASE', 1
CREATE PROCEDURE [dbo].[Ensayos_Get]
	@categoriaId INT = NULL
	, @productoId INT = NULL
	, @campanaId INT = NULL
	, @lugarId INT = NULL
	, @fuente VARCHAR(50) = NULL
	, @isChart BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ranking TABLE (Id INT, Ranking INT)
	INSERT INTO @ranking (Id, Ranking)
	Select en.Id, RANK() OVER (PARTITION BY en.[PlaceId], en.[CampaignId], en.[Source] ORDER BY en.[Yield] DESC) From [Tests] en
	Inner Join [Products] p on en.[ProductId] = p.Id
	Where p.[CategoryId] = @categoriaId
	
	DECLARE @total TABLE (LugarId INT, CampanaId INT, Fuente VARCHAR(50), Total INT)
	INSERT INTO @total (LugarId, CampanaId, Fuente, Total)
	Select en.[PlaceId], en.[CampaignId], en.[Source], COUNT([PlaceId]) Total From [Tests] en
	Inner Join [Products] p on en.[ProductId] = p.Id
	Where p.[CategoryId] = @categoriaId
	Group By [PlaceId], [CampaignId], [Source]
	
	DECLARE @ensayos TABLE (Id INT, CampanaId INT, Campana VARCHAR(50), ProductoId INT, Producto VARCHAR(50), LugarId INT, Provincia VARCHAR(50), Departamento VARCHAR(50), Localidad VARCHAR(50), Rinde SMALLMONEY, Fuente VARCHAR(50), Establecimiento VARCHAR(50), FechaSiembra DATE, FechaCosecha Date, Indice INT, Observaciones VARCHAR(100), Archivo VARCHAR(100), CategoriaId INT, Categoria VARCHAR(50), Ranking INT, Total INT)
	
	INSERT INTO @ensayos (Id, CampanaId, Campana, ProductoId, Producto, LugarId, Provincia, Departamento, Localidad, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo, CategoriaId, Categoria, Ranking, Total)
		SELECT e.Id, e.[CampaignId], c.[Name] Campana, e.[ProductId], p.[Name] Producto, e.[PlaceId], l.[Province], l.[Department], l.[Locality], e.[Yield], e.[Source], e.[Establishment], e.[PlantingDate], e.[HarvestDate], e.[Index], e.[Observations], e.[File] 
				, p.[CategoryId] , ca.[Name] Categoria, r.Ranking, t.Total
		From [Tests] e
		Inner Join [Products] p on e.[ProductId] = p.Id
		Inner Join [Categories] ca on ca.Id = p.[CategoryId]
		Inner Join [Campaigns] c on e.[CampaignId] = c.Id
		Inner Join @ranking r on e.Id = r.Id
		Inner Join @total t on e.[PlaceId] = t.LugarId and e.[CampaignId] = t.CampanaId and e.[Source] = t.Fuente
		Left Join [Places] l on e.[PlaceId] = l.Id
		Where 
			(@categoriaId IS NULL OR @categoriaId = 0 OR (@categoriaId IS NOT NULL AND p.[CategoryId] = @categoriaId))
			AND (@productoId IS NULL OR (@productoId IS NOT NULL AND e.[ProductId] = @productoId))
			AND (@campanaId IS NULL OR (@campanaId IS NOT NULL AND e.[CampaignId] = @campanaId))
			AND (@lugarId IS NULL OR (@lugarId IS NOT NULL AND e.[PlaceId] = @lugarId))
			AND (@fuente IS NULL OR (@fuente IS NOT NULL AND e.[Source] = @fuente))
		Order by e.[Yield] DESC, e.[CampaignId], e.[ProductId], l.[Province], l.[Department], l.[Locality]
			
		IF @isChart = 0
			Select * From @ensayos
		ELSE
			Select * From @ensayos
			Where Ranking <= 5
			UNION 
			Select * From (Select TOP 1 Id, CampanaId, Campana, ProductoId, 'ULTIMO' Producto, LugarId, Provincia, Departamento, Localidad, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo, CategoriaId, Categoria, Ranking, Total From @ensayos Order By Ranking DESC) data
			UNION
			Select 0 Id, CampanaId, Campana, 0 ProductoId, 'PROMEDIO', LugarId, Provincia, Departamento, Localidad, AVG(Rinde) Rinde, Fuente, '' Establecimiento, null FechaSiembra, null FechaCosecha, null Indice, null Observaciones, null Archivo, CategoriaId, Categoria, 0 Ranking, 0 Total
			From @ensayos
			Group By CampanaId, Campana, LugarId, Provincia, Departamento, Localidad, Fuente, CategoriaId, Categoria
			Order By Rinde DESC
	
END