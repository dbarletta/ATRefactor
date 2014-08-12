/* PRODUCTOS */
INSERT INTO Productos (CategoriaId, EmpresaId, Nombre, DescripcionPG, Material, EsHibrido, Ciclo, EsConvencional, DiasFloracion, DiasMadurez, AlturaPlanta, EsNuevo, Alta, FechaCarga, Deshabilitado)
Select 
	c.Id CategoriaId
	, e.Id EmpresaId
	, t.Producto Nombre
	, t.Descripcion DescripcionPG
	, t.Material
	, CASE WHEN [Hibrido] = 'S' THEN 1 WHEN [Hibrido] = 'N' THEN 0 ELSE NULL END AS EsHibrido
	, t.Ciclo
	, CASE WHEN [Convencional] = 'S' THEN 1 WHEN [Convencional] = 'N' THEN 0 ELSE NULL END AS EsConvencional
	, CASE WHEN t.[Dias a Floracion] like '%/%' THEN 0 WHEN t.[Dias a Floracion] like 's/d' THEN 0 WHEN t.[Dias a Floracion] LIKE '%-%' THEN 0 WHEN CAST(t.[Dias a Floracion] AS VARCHAR) IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Floracion] AS INT) END AS DiasFloracion
	, CASE WHEN t.[Dias a Madurez Fisiologica] like '%/%' THEN 0 WHEN t.[Dias a Madurez Fisiologica] like 's/d' THEN 0 WHEN CAST(t.[Dias a Madurez Fisiologica] AS VARCHAR) LIKE '%-%' THEN 0 WHEN t.[Dias a Madurez Fisiologica] LIKE '% a %' THEN 0  WHEN CAST(t.[Dias a Madurez Fisiologica] AS VARCHAR) IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Madurez Fisiologica] AS INT) END AS DiasMadurez
	, CASE WHEN t.[Altura de Planta (cm)] like '%/%' THEN 0 WHEN t.[Altura de Planta (cm)] = 's/d' THEN 0 WHEN t.[Altura de Planta (cm)] in ('Alta','Baja','Media') THEN 0 WHEN t.[Altura de Planta (cm)] LIKE '%-%' THEN 0 WHEN t.[Altura de Planta (cm)] IN ('-', ' ') THEN NULL ELSE CAST(t.[Altura de Planta (cm)] AS INT) END AS AlturaPlanta
	, CASE WHEN [Nuevo en 2013?] = 'S' THEN 1 ELSE 0 END AS EsNuevo
	, CAST([Fecha Alta Producto] AS INT) Alta
	, CAST([Fecha Carga] AS DATE) FechaCarga
	, CASE WHEN [Fecha Carga] is null THEN 1 ELSE 0 END AS Deshabilitado
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Inner Join Empresas e on ISNULL(t.empresa, 'SIN EMPRESA') = e.Nombre


/* ATRIBUTOS */

--RSF
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'RSF') AtributoId
	, Valor = CASE WHEN [RSF] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([RSF] AS VARCHAR) NOT IN ('s/d', ' ', '') and [RSF] is not null

--RG
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'RG') AtributoId
	, Valor = CASE WHEN [RG] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([RG] AS VARCHAR) NOT IN ('s/d', ' ', '') and [RG] is not null

--Intacta RR2 Pro
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Intacta RR2 Pro') AtributoId
	, Valor = CASE WHEN [Intacta RR2 Pro] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Intacta RR2 Pro] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Intacta RR2 Pro] is not null


--STS
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'STS') AtributoId
	, Valor = CASE WHEN [STS] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([STS] AS VARCHAR) NOT IN ('s/d', ' ', '') and [STS] is not null


--Habito de Crecimiento
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Habito de Crecimiento') AtributoId
	, Valor = [Habito de Crecimiento]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Habito de Crecimiento] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Habito de Crecimiento] is not null


--Disp. Trat Prof de sem
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Disp. Trat Prof de sem') AtributoId
	, Valor = CASE WHEN [Disp Trat Prof de sem] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Disp Trat Prof de sem] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Disp Trat Prof de sem] is not null

--Color de Flor
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Color de Flor') AtributoId
	, Valor = [Color de Flor]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Color de Flor] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Color de Flor] is not null

--Tipo de planta
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Tipo de planta') AtributoId
	, Valor = [Tipo de planta]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Tipo de planta] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Tipo de planta] is not null

--Cancro del tallo
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Cancro del tallo') AtributoId
	, Valor = [Cancro del tallo]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Cancro del tallo] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Cancro del tallo] is not null

--Nematode del quiste
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Nematode del quiste') AtributoId
	, Valor = [Nematode del quiste]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Nematode del quiste] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Nematode del quiste] is not null

--Nematode de agalla
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Nematode de agalla') AtributoId
	, Valor = [Nematode de agalla]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Nematode de agalla] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Nematode de agalla] is not null

--Mancha ojo de rana (Cercospora Sojina)
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Mancha ojo de rana (Cercospora Sojina)') AtributoId
	, Valor = [Mancha ojo de rana (Cercospora Sojina)]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Mancha ojo de rana (Cercospora Sojina)] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Mancha ojo de rana (Cercospora Sojina)] is not null

--R1
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'R1') AtributoId
	, Valor = [R1]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([R1] AS VARCHAR) NOT IN ('s/d', ' ', '') and [R1] is not null

--R3
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'R3') AtributoId
	, Valor = [R3]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([R3] AS VARCHAR) NOT IN ('s/d', ' ', '') and [R3] is not null

--R17
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'R17') AtributoId
	, Valor = [R17]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([R17] AS VARCHAR) NOT IN ('s/d', ' ', '') and [R17] is not null

--R25
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'R25') AtributoId
	, Valor = [R25]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([R25] AS VARCHAR) NOT IN ('s/d', ' ', '') and [R25] is not null

--R4
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'R4') AtributoId
	, Valor = [R4]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([R4] AS VARCHAR) NOT IN ('s/d', ' ', '') and [R4] is not null

--tipo de ramificación
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'tipo de ramificación') AtributoId
	, Valor = [tipo de ramificación]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([tipo de ramificación] AS VARCHAR) NOT IN ('s/d', ' ', '') and [tipo de ramificación] is not null

--Susceptibilidad al vuelco
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Susceptibilidad al vuelco') AtributoId
	, Valor = [Susceptibilidad al vuelco]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Susceptibilidad al vuelco] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Susceptibilidad al vuelco] is not null

--Potencial de ramificación
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Potencial de ramificación') AtributoId
	, Valor = [Potencial de ramificación]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Potencial de ramificación] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Potencial de ramificación] is not null

--Color de pubescencia
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Color de pubescencia') AtributoId
	, Valor = [Color de pubescencia]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Color de pubescencia] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Color de pubescencia] is not null

--Grupo de Madurez
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Grupo de Madurez') AtributoId
	, Valor = [Grupo de Madurez]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Grupo de Madurez] AS VARCHAR) NOT IN ('s/d', ' ', '') and [Grupo de Madurez] is not null


/******************************************************** LUGARES ***********************************************************/
INSERT INTO dbo.ProductoLugares (ProductoId, LugarId)
--Bs As SUDESTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'BUENOS AIRES SUDESTE' AND t.BSE = 'S'

UNION

--Bs As SUDOESTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'BUENOS AIRES SUDOESTE' AND t.BSO = 'S'

UNION
--Bs As OESTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'BUENOS AIRES OESTE' AND t.BO = 'S'

UNION
--Bs As Norte
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'BUENOS AIRES NORTE' AND t.BN = 'S'
	
UNION
--Bs As Centro
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'BUENOS AIRES CENTRO' AND t.BC = 'S'
	
UNION
--Cordoba NORTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'CORDOBA NORTE' AND t.CN = 'S'
	
UNION
--Cordoba SUDESTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'CORDOBA SUDESTE' AND t.CSE = 'S'
	
UNION
--Cordoba SUR
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'CORDOBA SUR' AND t.CS = 'S'

UNION
--Santa Fe NORTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'SANTA FE NORTE' AND t.SN = 'S'

UNION
--Santa Fe CENTRO
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'SANTA FE CENTRO' AND t.SC = 'S'
	
UNION
--Santa Fe SUR
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'SANTA FE SUR' AND t.SS = 'S'
	
UNION
--ENTRE RIOS
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'ENTRE RIOS' AND t.ER = 'S'
	
UNION
--LA PAMPA
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'LA PAMPA' AND t.LP = 'S'

UNION
--NEA
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'NEA' AND t.NEA = 'S'

UNION
--NOA
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'NOA' AND t.NOA = 'S'

UNION
--SAN LUIS
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'SAN LUIS' AND t.SL = 'S'
	
/*
Delete From ProductoAtributos
Delete From ProductoLugares
Delete From Productos
Delete From AtributoEquivalencias
DBCC CHECKIDENT('Productos', RESEED, 0) 
*/


/*
INSERT INTO Empresas (Nombre, Deshabilitada)
Select 
	distinct UPPER(RTRIM(LTRIM(t.empresa)))
	, 1
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Soja\Soja.xls', 
                'SELECT * FROM [Sheet1$]') t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Left Join Empresas e on ISNULL(t.empresa, 'SIN EMPRESA') = e.Nombre
Where e.Id is null
*/