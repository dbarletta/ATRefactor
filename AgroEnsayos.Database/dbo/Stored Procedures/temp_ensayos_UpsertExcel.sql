--Truncate Table Ensayos
--Select * From Ensayos
--temp_ensayos_UpsertExcel 4
CREATE PROCEDURE [dbo].[temp_ensayos_UpsertExcel]
@categoriaId INT
AS
BEGIN

DECLARE @ensayos TABLE (CampanaId INT, ProductoId INT, LugarId INT, Rinde SMALLMONEY, Fuente VARCHAR(50), Establecimiento VARCHAR(50), FechaSiembra DATE, FechaCosecha DATE, Indice INT, Observaciones VARCHAR(100), Archivo VARCHAR(100))
INSERT INTO @ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)
Select * From
(
	select 
		camp.Id CampanaId
		, prod.Id ProductoId
		, CASE WHEN l.Id is not null THEN l.Id ELSE l2.Id END AS LugarId
		, ROUND(t.[Yield],0) Rinde
		, UPPER(t.[Source]) Fuente
		, t.[Establishment]
		, t.[PlantingDate]
		, t.[HarvestDate]
		, t.[Index]
		, t.[Observations]
		, t.[File]
	from [Temp_Tests] t
	inner join [Products] prod on prod.[Name] = LTRIM(RTRIM(t.[Product]))
	inner join [Campaigns] camp on camp.[Name] = t.[Campaign] and camp.[CategoryId] = @categoriaId
	Left Join [Places] l on t.[Province] = l.[Province] and t.[Locality] = l.[Locality] and l.[Locality] is not null
	Left Join [Places] l2 on l.Id is null and t.[Province] = l2.[Province] and t.[Locality] = l2.[Department] and l2.[Department] is not null and l2.[Locality] is null
) as Tabla1
Where 
	LugarId is not null and Rinde is not null and Fuente is not null and [PlantingDate] is not null and CampanaId is not null and ProductoId is not null 

--Update
Update [Tests]
Set [Yield] = e2.Rinde, [Establishment] = e2.Establecimiento, [PlantingDate] = e2.FechaSiembra, [HarvestDate] = e2.FechaCosecha, [Index] = e2.Indice, [File] = e2.Archivo
From [Tests] e1
Inner Join @ensayos e2 on e1.[CampaignId] = e2.CampanaId and e1.[ProductId] = e2.ProductoId and e1.[PlaceId] = e2.LugarId and e1.[Source] = e2.Fuente and ISNULL(e1.[Observations],'1') = ISNULL(e2.Observaciones,'1')
Where 
	e2.Rinde != e1.[Yield]
	OR e2.Establecimiento != e1.[Establishment]
	OR e2.FechaSiembra != e1.[PlantingDate]
	OR e2.FechaCosecha != e1.[HarvestDate]
	OR e2.Indice != e1.[Index]
	OR e2.Archivo != e1.[File]
	OR e2.Observaciones != e1.[Observations]
	
--Insert	
INSERT INTO [Tests] ([CampaignId], [ProductId], [PlaceId], [Yield], [Source], [Establishment], [PlantingDate], [HarvestDate], [Index], [Observations], [File])
Select e1.CampanaId, e1.ProductoId, e1.LugarId, e1.Rinde, e1.Fuente, e1.Establecimiento, e1.FechaSiembra, e1.FechaCosecha, e1.Indice, e1.Observaciones, e1.Archivo From @ensayos e1
Left Join [Tests] e2 on e1.CampanaId = e2.[CampaignId] and e1.ProductoId = e2.[ProductId] and e1.LugarId = e2.[PlaceId] and e1.Fuente = e2.[Source] and ISNULL(e1.Observaciones,'1') = ISNULL(e2.[Observations],'1')
Where e2.Id is null

--Borro Tabla temp_ensayos
Truncate Table [Temp_Tests]


END