CREATE PROCEDURE [dbo].[temp_ensayos_Validations]
@categoriaId INT
AS
BEGIN
	--Datos Obligatarios
    select distinct
		1 TipoError
		, 2 Severity
		, 'Alguno de los datos obligatorios está faltando (Fuente, Provincia, Localidad, Campaña, Producto, Fecha de siembra o Rinde)' Description
		, [Source] Param1
		, [Campaign] Param2
		, [Product] Param3
		, [Yield] Param4
		, [Province] + ' ' + [Locality] Param5
		, Row 
	from [Temp_Tests] 
	where 
		[Campaign] is null or [Campaign] = '' or [Campaign] = ' '  or [Campaign] = 's/d'  
		or [Product] is null or [Product] = '' or [Product] = ' '  or [Product] = 's/d'  
		or [Yield] is null or [Yield] = 0  
		or [Source] is null or [Source] = '' or [Source] = ' ' or [Source] = 's/d'
		or [Province] is null or [Province] = '' or [Province] = ' ' or [Province] = 's/d'
		or [Locality] is null or [Locality] = '' or [Locality] = ' ' or [Locality] = 's/d'
		or [PlantingDate] is null
	UNION
	
	--Productos
	select distinct
		2 TipoError
		, 2 Severity
		, 'El Producto ' + e.[Product] + ' no se encontró en la Aplicación' Description
		, e.[Product] Param1
		, '' Param2
		, '' Param3
		, '' Param4
		, '' Param5
		, Row 
	from [Temp_Tests] e
	Left Join [Products] p on RTRIM(LTRIM(e.[Product])) = p.[Name]
	Where p.Id is null and e.[Product] is not null and e.[Product] != '' and e.[Product] != ' ' and e.[Product] != 's/d'
	
	UNION
	
	--Campaña
	select distinct
		2 TipoError
		, 2 Severity
		, 'La Campaña ' + e.[Campaign] + ' de ' + (Select [Name] From [Categories] Where Id = @categoriaId) + ' no se encontró en la Aplicación' Description
		, e.[Campaign] Param1
		, '' Param2
		, '' Param3
		, '' Param4
		, '' Param5
		, Row 
	from [Temp_Tests] e
	Left Join [Campaigns] camp on camp.[Name] = e.[Campaign] and camp.[CategoryId] = @categoriaId
	Where camp.Id is null and e.[Campaign] is not null and e.[Campaign] != '' and e.[Campaign] != ' ' and e.[Campaign] != 's/d'
	
	UNION 
	
	--Lugar
	Select distinct * From 
	(
	select distinct
		2 TipoError
		, 2 Severity
		, 'El Lugar compuesto por la Provincia de ' + e.[Province] + ' y en la Localidad / Departamento de ' + e.[Locality] + ' no se encontró en la Aplicación' Description
		, e.[Province] Param1
		, e.[Locality] Param2
		, CASE WHEN l.Id is not null THEN CAST(l.Id AS VARCHAR(10)) ELSE CAST(l2.Id AS VARCHAR(10)) END AS Param3
		, '' Param4
		, '' Param5
		, Row 
	from [Temp_Tests] e
	Left Join [Places] l on e.[Province] = l.[Province] and e.[Locality] = l.[Locality] and l.[Locality] is not null
	Left Join [Places] l2 on l.Id is null and e.[Province] = l2.[Province] and e.[Locality] = l2.[Department] and l2.[Department] is not null and l2.[Locality] is null
	Where 
		(l.Id is null or l2.Id is null) 
		and e.[Province] is not null and e.[Province] != '' and e.[Province] != ' ' and e.[Province] != 's/d'
		and e.[Locality] is not null and e.[Locality] != '' and e.[Locality] != ' ' and e.[Locality] != 's/d'
	) data
	Where Param3 is null
	
	--Repetidos con distinto Rinde (en muchos casos tienen distinta fecha de siembra) Hay que ver con Martin que hacemos en estos casos.
	UNION
	
	Select 
		3 TipoError
		, 2 Severity
		, 'Este dato se encuentra repetido' Description
		, [Source] Param1
		, [Campaign] Param2
		, [Province] Param3
		, [Locality] Param4
		, [Product] Param5
		, Repetidos  
	From (
		select [Source], [Campaign], [Province], [Locality], [Product], COUNT([Source]) Repetidos From (
		select [Source], [Campaign], [Province], [Locality], [Product], [Yield] From [Temp_Tests]
		where 
		[Campaign] is null or [Campaign] = '' or [Campaign] = ' '  or [Campaign] = 's/d'  
		or [Product] is null or [Product] = '' or [Product] = ' '  or [Product] = 's/d'  
		or [Yield] is null or [Yield] = 0  
		or [Source] is null or [Source] = '' or [Source] = ' ' or [Source] = 's/d'
		or [Province] is null or [Province] = '' or [Province] = ' ' or [Province] = 's/d'
		or [Locality] is null or [Locality] = '' or [Locality] = ' ' or [Locality] = 's/d'

		Group By [Source], [Campaign], [Province], [Locality], [Product], [Yield]) data
		Group By [Source], [Campaign], [Province], [Locality], [Product]
		Having Count([Source]) > 1
	) data
		
	
	
END

