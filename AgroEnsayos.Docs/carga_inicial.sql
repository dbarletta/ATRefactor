INSERT INTO Productos (CategoriaId, EmpresaId, Producto, Material, EsHibrido, Ciclo, EsConvencional, DiasFloracion, DiasMadurez)
SELECT 
	c.Id CategoriaId
	, e.Id EmpresaId
	, t.Producto
	, t.Material
	, CASE 
		WHEN EsHibrido = 'N' THEN 0
		WHEN EsHibrido = 'S' THEN 1
		ELSE NULL
	END As EsHibrido
	, t.Ciclo
	, CASE 
		WHEN EsConvencional = 'N' THEN 0
		WHEN EsConvencional = 'S' THEN 1
		ELSE NULL
	END As EsConvencional
	, CASE WHEN DiasFloracion IS NULL OR DiasFloracion = 0 THEN NULL ELSE CAST(t.DiasFloracion AS INT) END AS DiasFloracion
	, CASE WHEN DiasMadurez IS NULL OR DiasMadurez = 0 THEN NULL ELSE CAST(t.DiasMadurez AS INT) END AS DiasMadurez
FROM temp_productos t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Inner Join Empresas e on t.empresa = e.Nombre


--LUGARES--
INSERT INTO Lugares (Region, Provincia, Departamento, Cabecera, Perimetro)
SELECT
	CASE 
		WHEN Provincia = 'LA PAMPA' THEN 'LA PAMPA'
		When Provincia = 'SAN LUIS' THEN 'SAN LUIS'
		When Provincia = 'ENTRE RIOS' THEN 'ENTRE RIOS'
		When Provincia in ('Jujuy', 'Salta', 'Tucuman', 'Catamarca', 'La Rioja', 'Santiago del Estero') THEN 'NOA'
		When Provincia in ('FORMOSA', 'CHACO', 'Corrientes', 'Misiones') THEN 'NEA'
		When Provincia in ('CHUBUT', 'RIO NEGRO', 'NEUQUEN', 'SANTA CRUZ', 'TIERRA DEL FUEGO') THEN 'PATAGONIA'
		When Provincia in ('MENDOZA', 'SAN JUAN') THEN 'CUYO'
	 END As Region
	,[Provincia]
	, [Departamento]
    ,[Cabecera]
    ,[Geometry]
  FROM [AgroEnsayos].[dbo].[temp_lugares]
  
 