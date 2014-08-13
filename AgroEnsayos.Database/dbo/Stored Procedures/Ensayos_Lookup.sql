
CREATE PROCEDURE [dbo].[Ensayos_Lookup]
	@categoriaId INT
	, @searchTerm VARCHAR(100) = ''
	, @empresa VARCHAR(100)
	, @fuente VARCHAR(100)
	, @provincia VARCHAR(200)
	, @localidad VARCHAR(200)
	, @campana VARCHAR(200)
	, @atributoTbl AtributoEquivalenciasType READONLY
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
	
	SELECT en.Id, p.[CategoryId], c.[Name] Categoria, en.[Source]
		, e.[Name] Empresa, p.[Name] Producto, p.[Description]
		, en.[CampaignId] , camp.[Name] Campana, en.[Yield] , en.[Establishment] , en.[HarvestDate]
		, en.[PlantingDate] , en.[Index] , en.[ProductId] , en.[Observations] , en.[PlaceId] , l.[Department]
		, en.[File], l.[Province] , ISNULL(l.[Locality],l.[Department]) localidad  
		, r.Ranking
		, t.Total
	From [Tests] en
	Inner Join [Products] p on en.[ProductId] = p.Id
	Inner Join [Places] l on en.[PlaceId] = l.Id
	Inner Join [Companies] e on p.[CompanyId] = e.Id
	Inner Join [Categories] c on p.[CategoryId] = c.Id
	Inner join [Campaigns] camp on en.[CampaignId] = camp.Id
	Inner Join @ranking r on en.Id = r.Id
	Inner Join @total t on en.[PlaceId] = t.LugarId and en.[CampaignId] = t.CampanaId and en.[Source] = t.Fuente
	left join (select distinct pa.[ProductId]  
				from [ProductAttribute] pa 
					inner join [AttributeMappings] ae on (pa.[AttributeId] = ae.[AttributeId] and pa.[OriginalValue] = ae.[OriginalValue]  ) 
					inner join @atributoTbl atbl on (atbl.AtributoId = ae.[AttributeId] and atbl.Equivalencia = ae.[MappedValue])
				) as atrp on ( p.Id = atrp.[ProductId] )
	Where p.[CategoryId] = @categoriaId
	AND (atrp.[ProductId] is not null or (select COUNT(*) from @atributoTbl)=0 )
	AND (@empresa = '' OR CHARINDEX(','+cast(e.Id as varchar)+',', @empresa) > 0  )
	AND (@fuente = '' OR CHARINDEX(','+cast(en.[Source] as varchar)+',', @fuente) > 0   )
	AND (@provincia = '' OR CHARINDEX(','+cast(l.[Province] as varchar)+',', @provincia) > 0   )
	AND (@localidad = '' OR CHARINDEX(','+cast(l.[Locality] as varchar)+',', @localidad) > 0   )
	AND (@campana = '' OR (@campana != '' AND en.[CampaignId] in (select c.Id from [Campaigns] c where c.[CategoryId] = @categoriaId and CHARINDEX(',' + cast(c.Id as VARCHAR) + ',', @campana) > 0 ))) 
	
	AND (@searchTerm = '' OR (@searchTerm != '' AND (CHARINDEX(@searchTerm, p.[Name]) > 0 
		OR CHARINDEX(@searchTerm, en.[Source]) > 0
		OR CHARINDEX(@searchTerm, e.[Name]) > 0
		OR CHARINDEX(@searchTerm, l.[Locality]) > 0
		OR CHARINDEX(@searchTerm, l.[Department]) > 0
		OR p.Id in (
			Select distinct pa.[ProductId] From [ProductAttribute] pa
			Inner Join [AttributeCategory] ac on pa.[AttributeId] = ac.[AttributeId]
			Inner Join [Attributes] a on ac.[AttributeId] = a.Id
			Where ac.[CategoryId] = @categoriaId and CHARINDEX(@searchTerm, a.Tags) > 0
			UNION
			Select distinct pl.[ProductId] From [ProductPlace] pl
			Inner Join [Products] p on pl.[ProductId] = p.Id and p.[CategoryId] = @categoriaId
			Inner Join [Places] l on pl.[PlaceId] = l.Id
			Where
				CHARINDEX(@searchTerm, l.Region) > 0
				OR CHARINDEX(@searchTerm, l.[Province]) > 0
				OR CHARINDEX(@searchTerm, l.[Department]) > 0
				OR CHARINDEX(@searchTerm, l.[Header]) > 0)))) 
				
END
