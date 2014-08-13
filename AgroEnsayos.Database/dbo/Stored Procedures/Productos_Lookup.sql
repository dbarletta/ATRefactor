/*
	DECLARE @atributoTbl AtributoEquivalenciasType
	EXEC Productos_Lookup 4, '', '', '', 'BUENOS AIRES CENTRO', @atributoTbl
*/
CREATE PROCEDURE [dbo].[Productos_Lookup]
	@categoriaId INT
	, @searchTerm VARCHAR(100) = ''
	, @empresa VARCHAR(100)
	, @antiguedad VARCHAR(100)
	, @region VARCHAR(200)
	, @atributoTbl AtributoEquivalenciasType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.[CategoryId], c.[Name] Categoria, p.[CompanyId], e.[Name] Empresa, p.[Name], p.[Description], p.Material, p.[IsHybrid], p.[Cycle], p.[IsConventional], p.[DaysToFlowering], p.[DaysToMaturity], p.[PlantHeight], p.[IsNew], p.[Height], p.[EntryDate] 
	From [Products] p
	Inner Join [Companies] e on p.[CompanyId] = e.Id
	Inner Join [Categories] c on p.[CategoryId] = c.Id
	left join (select distinct pa.[ProductId] productoId 
				from [ProductAttribute] pa 
					inner join [AttributeMappings] ae on (pa.[AttributeId] = ae.[AttributeId] and pa.[OriginalValue] = ae.[OriginalValue]  ) 
					inner join @atributoTbl atbl on (atbl.AtributoId = ae.[AttributeId] and atbl.Equivalencia = ae.[MappedValue])
				) as atrp on ( p.Id = atrp.[ProductId] )
	Where [CategoryId] = @categoriaId
	AND (e.[IsDisabled] = 0) --Agregado para que no traiga productos de empresas deshabilitadas
	AND (atrp.[ProductId] is not null or (select COUNT(*) from @atributoTbl)=0 )
	AND (@empresa = '' OR CHARINDEX(','+cast(e.Id as varchar)+',', @empresa) > 0  )
	AND (@antiguedad = '' OR CHARINDEX(','+cast(p.[IsNew] as varchar)+',', @antiguedad) > 0   )
	AND (@region = '' OR (@region != '' AND p.Id in (select pl.[ProductId] from [ProductPlace] pl, [Places] l where pl.[PlaceId] = l.Id and CHARINDEX(',' + l.Region + ',', @region) > 0 and l.[Department] is null))) 
	AND (@searchTerm = '' OR (@searchTerm != '' AND (CHARINDEX(@searchTerm, p.[Name]) > 0 
		OR CHARINDEX(@searchTerm, p.Material) > 0
		OR CHARINDEX(@searchTerm, e.[Name]) > 0
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
