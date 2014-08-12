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
	, CASE WHEN t.[Dias a Floracion] like '%/%' THEN 0 WHEN t.[Dias a Floracion] = 's/d' THEN 0 WHEN t.[Dias a Floracion] LIKE '%-%' THEN 0 WHEN t.[Dias a Floracion] IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Floracion] AS INT) END AS DiasFloracion
	, CASE WHEN t.[Dias a Madurez Fisiologica] like '%/%' THEN 0 WHEN t.[Dias a Madurez Fisiologica] = 's/d' THEN 0 WHEN t.[Dias a Madurez Fisiologica] LIKE '%-%' THEN 0 WHEN t.[Dias a Madurez Fisiologica] LIKE '% a %' THEN 0  WHEN t.[Dias a Madurez Fisiologica] IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Madurez Fisiologica] AS INT) END AS DiasMadurez
	, CASE WHEN t.[Altura de Planta (cm)] like '%/%' THEN 0 WHEN t.[Altura de Planta (cm)] = 's/d' THEN 0 WHEN t.[Altura de Planta (cm)] LIKE '%-%' THEN 0 WHEN t.[Altura de Planta (cm)] IN ('-', ' ') THEN NULL ELSE CAST(t.[Altura de Planta (cm)] AS INT) END AS AlturaPlanta
	, CASE WHEN [Nuevo en 2013?] = 'S' THEN 1 ELSE 0 END AS EsNuevo
	, CAST([Fecha Alta Producto] AS INT) Alta
	, CAST([Fecha Carga] AS DATE) FechaCarga
	, CASE WHEN [Fecha Carga] is null THEN 1 ELSE 0 END AS Deshabilitado
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Inner Join Empresas e on ISNULL(t.empresa, 'SIN EMPRESA') = e.Nombre

/* ATRIBUTOS */

--Nicosulfuron
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Nicosulfuron (Challenger)') AtributoId
	, Valor = CASE WHEN [Nicosulfuron (Challenger)] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Nicosulfuron (Challenger)] NOT IN ('s/d', ' ', '') and [Nicosulfuron (Challenger)] is not null

--Callisto (mesotrione)
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Callisto (mesotrione)') AtributoId
	, Valor = CASE WHEN [Callisto (mesotrione)] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Callisto (mesotrione)] NOT IN ('s/d', ' ', '') and [Callisto (mesotrione)] is not null

--Equip Foramsulfurón + Iodosulfurón 
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Equip Foramsulfurón + Iodosulfurón') AtributoId
	, Valor = CASE WHEN [Equip Foramsulfurón + Iodosulfurón] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Equip Foramsulfurón + Iodosulfurón] NOT IN ('s/d', ' ', '') and [Equip Foramsulfurón + Iodosulfurón] is not null


select * from ProductoAtributos where AtributoId = 20

--HCL
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'HCL') AtributoId
	, Valor = CASE WHEN [HCL] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [HCL] NOT IN ('s/d', ' ', '') and [HCL] is not null

--CL
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'CL') AtributoId
	, Valor = CASE WHEN [CL] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [CL] NOT IN ('s/d', ' ', '') and [CL] is not null


--BT Generico
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'BT') AtributoId
	, Valor = CASE WHEN [BT Generico] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [BT Generico] NOT IN ('s/d', ' ', '') and [BT Generico] is not null

--RR2
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'RR2') AtributoId
	, Valor = CASE WHEN [RR2] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [RR2] NOT IN ('s/d', ' ', '') and [RR2] is not null

--TG Plus
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'TG Plus') AtributoId
	, Valor = CASE WHEN [TG Plus] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [TG Plus] NOT IN ('s/d', ' ', '') and [TG Plus] is not null


--LIB
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'LIB') AtributoId
	, Valor = CASE WHEN [LIB] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [LIB] NOT IN ('s/d', ' ', '') and [LIB] is not null

--TD Max
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'TD Max') AtributoId
	, Valor = CASE WHEN [TD Max] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [TD Max] NOT IN ('s/d', ' ', '') and [TD Max] is not null

--MG
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'MG') AtributoId
	, Valor = CASE WHEN [MG] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [MG] NOT IN ('s/d', ' ', '') and [MG] is not null

--HX
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'HX') AtributoId
	, Valor = CASE WHEN [HX] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [HX] NOT IN ('s/d', ' ', '') and [HX] is not null

--Viptera
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Viptera') AtributoId
	, Valor = CASE WHEN [Viptera] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Viptera] NOT IN ('s/d', ' ', '') and [Viptera] is not null

--Viptera 2
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Viptera 2') AtributoId
	, Valor = CASE WHEN [Viptera 2] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Viptera 2] NOT IN ('s/d', ' ', '') and [Viptera 2] is not null

--Viptera 3
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Viptera 3') AtributoId
	, Valor = CASE WHEN [Viptera 3] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Viptera 3] NOT IN ('s/d', ' ', '') and [Viptera 3] is not null

--POWER CORE
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'POWER CORE (5 Traits DOW)') AtributoId
	, Valor = CASE WHEN [POWER CORE (5 Traits DOW)] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [POWER CORE (5 Traits DOW)] NOT IN ('s/d', ' ', '') and [POWER CORE (5 Traits DOW)] is not null

--Vt pro
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Vt pro') AtributoId
	, Valor = CASE WHEN [Vt pro] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where [Vt pro] NOT IN ('s/d', ' ', '') and [Vt pro] is not null

--% Refugio
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = '% Refugio') AtributoId
	, Valor = [% Refugio]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([% Refugio] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [% Refugio] is not null

--ICR
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'ICR') AtributoId
	, Valor = [ICR]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([ICR] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [ICR] is not null

--GDU En Floracion
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'GDU En Floracion') AtributoId
	, Valor = [GDU En Floracion]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([GDU En Floracion] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [GDU En Floracion] is not null

--Cruzamiento
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Cruzamiento') AtributoId
	, Valor = [Cruzamiento]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Cruzamiento] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Cruzamiento] is not null

--Templado/Tropical
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Templado/Tropical') AtributoId
	, Valor = [Templado/Tropical]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Templado/Tropical] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Templado/Tropical] is not null

--Color de grano / Tipo de grano
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Color de grano / Tipo de grano') AtributoId
	, Valor = [Color de grano / Tipo de grano]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Color de grano / Tipo de grano] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Color de grano / Tipo de grano] is not null

--Mal Rio 4
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Mal Rio 4') AtributoId
	, Valor = [Mal Rio 4]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Mal Rio 4] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Mal Rio 4] is not null

--Roya comun
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Roya comun') AtributoId
	, Valor = [Roya comun]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Roya comun] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Roya comun] is not null

--Hongos espiga
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Hongos espiga') AtributoId
	, Valor = [Hongos espiga]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Hongos espiga] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Hongos espiga] is not null

--Helmintosporium Maydis (Tizon)
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Helmintosporium Maydis (Tizon)') AtributoId
	, Valor = [Helmintosporium Maydis (Tizon)]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Helmintosporium Maydis (Tizon)] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Helmintosporium Maydis (Tizon)] is not null

--Exserohilum Turcicum Tizon
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Setosphaeria turcica / Exserohilum Turcicum Tizon') AtributoId
	, Valor = [Setosphaeria turcica / Exserohilum Turcicum Tizon]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Setosphaeria turcica / Exserohilum Turcicum Tizon] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Setosphaeria turcica / Exserohilum Turcicum Tizon] is not null


--Corn Stunt
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Corn Stunt') AtributoId
	, Valor = [Corn Stunt]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Corn Stunt] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Corn Stunt] is not null

--Poncho
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Poncho') AtributoId
	, Valor = CASE WHEN [Poncho] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Poncho] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Poncho] is not null

--Maxxim Quattro
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Maxxim Quattro') AtributoId
	, Valor = CASE WHEN [Maxxim Quattro] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Maxxim Quattro] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Maxxim Quattro] is not null

--Maxxim XL Semillero
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Maxxim XL Semillero') AtributoId
	, Valor = CASE WHEN [Maxxim XL Semillero] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Maxxim XL Semillero] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Maxxim XL Semillero] is not null

--Cruiser		
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Cruiser') AtributoId
	, Valor = CASE WHEN [Cruiser] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Cruiser] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Cruiser] is not null

--Avicta
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Avicta') AtributoId
	, Valor = CASE WHEN [Avicta] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Avicta] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Avicta] is not null

--Acceleron
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Acceleron') AtributoId
	, Valor = CASE WHEN [Acceleron] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Acceleron] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Acceleron] is not null

--Vuelco
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Vuelco') AtributoId
	, Valor = [Vuelco]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Vuelco] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Vuelco] is not null

--Comportamiento a Quebrado
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Comportamiento a Quebrado') AtributoId
	, Valor = [Comportamiento a Quebrado]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Comportamiento a Quebrado] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Comportamiento a Quebrado] is not null

--Altura Insersion espiga (cm)
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Altura Insersion espiga (cm)') AtributoId
	, Valor = [Altura Insersion espiga (cm)]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Altura Insersion espiga (cm)] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Altura Insersion espiga (cm)] is not null

--Densidad  recomendada Secano	
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Densidad  recomendada Secano') AtributoId
	, Valor = [Densidad  recomendada Secano]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Densidad  recomendada Secano] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Densidad  recomendada Secano] is not null

--Densidad recomendada riego
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Densidad recomendada riego') AtributoId
	, Valor = [Densidad recomendada riego]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Producto = p.Nombre
Where CAST([Densidad recomendada riego] AS VARCHAR(100)) NOT IN ('s/d', ' ', '') and [Densidad recomendada riego] is not null



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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=D:\dev\iantech\Development\AgroEnsayos\src\AgroEnsayos.Docs\Imports\20140321\Productos\Maiz\Maiz.xls', 
                'SELECT * FROM [Sheet1$]') t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Left Join Empresas e on ISNULL(t.empresa, 'SIN EMPRESA') = e.Nombre
Where e.Id is null
*/