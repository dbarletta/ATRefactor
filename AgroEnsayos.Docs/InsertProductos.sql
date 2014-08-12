/* PRODUCTOS */

/* Chequeo cuales son las empresas que no existen */
Select 
	distinct t.Empresa
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Development\Agrotool\AgroEnsayos.Docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t
Left Join Empresas e on t.empresa = e.Nombre
Where e.Nombre is null



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
	, CASE WHEN t.[Dias a Floracion] = 's/d' THEN 0 WHEN t.[Dias a Floracion] IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Floracion] AS INT) END AS DiasFloracion
	, CASE WHEN t.[Dias a Madurez Fisiologica] = 's/d' THEN 0 WHEN t.[Dias a Madurez Fisiologica] IN ('-', ' ') THEN NULL ELSE CAST(t.[Dias a Madurez Fisiologica] AS INT) END AS DiasMadurez
	, CASE WHEN t.[Altura de Planta (cm)] = 's/d' THEN 0 WHEN t.[Altura de Planta (cm)] IN ('-', ' ') THEN NULL ELSE CAST(t.[Altura de Planta (cm)] AS INT) END AS AlturaPlanta
	, CASE WHEN [Nuevo en 2013?] = 'S' THEN 1 ELSE 0 END AS EsNuevo
	, CAST([Fecha Alta Producto] AS INT) Alta
	, CAST([Fecha Carga] AS DATE) FechaCarga
	, CASE WHEN [Fecha Carga] is null THEN 1 ELSE 0 END AS Deshabilitado
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t
Inner Join Categorias c on t.Subcategoria = c.Nombre
Inner Join Empresas e on ISNULL(t.empresa, 'SIN EMPRESA') = e.Nombre




/* ATRIBUTOS */
INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Capacidad de Macollaje') AtributoId
	, Valor = [Capacidad de macollaje]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Capacidad de macollaje] NOT IN ('s/d', ' ', '') and [Capacidad de macollaje] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Clearfield') AtributoId
	, Valor = CASE WHEN [Clearfield] = 'S' THEN 1 ELSE 0 END
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Clearfield] NOT IN ('s/d', ' ', '') and [Clearfield] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Porte Vegetativo') AtributoId
	, Valor = [Porte Vegetativo]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Porte Vegetativo] NOT IN ('s/d', ' ', '') and [Porte Vegetativo] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Caña') AtributoId
	, Valor = [Caña]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Caña] NOT IN ('s/d', ' ', '') and [Caña] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Comportamiento a Desgrane') AtributoId
	, Valor = [Comportamiento a  Desgrane]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Comportamiento a  Desgrane] NOT IN ('s/d', ' ', '') and [Comportamiento a  Desgrane] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Comportamiento Frente a Vuelco') AtributoId
	, Valor = [Comportamieinto Frente a Vuelco]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Comportamieinto Frente a Vuelco] NOT IN ('s/d', ' ', '') and [Comportamieinto Frente a Vuelco] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Espiga') AtributoId
	, Valor = [Espiga]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Espiga] NOT IN ('s/d', ' ', '') and [Espiga] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Densidad Recomendada Secano pl/m2') AtributoId
	, Valor = [Densidad Recomendada Secano pl/m2]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Densidad Recomendada Secano pl/m2] NOT IN ('s/d', ' ', '') and [Densidad Recomendada Secano pl/m2] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Densidad Recomendada Riego') AtributoId
	, Valor = [Densidad Recomendada Riego]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Densidad Recomendada Riego] NOT IN ('s/d', ' ', '') and [Densidad Recomendada Riego] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Grupo de Calidad 1 a 3') AtributoId
	, Valor = [Grupo de Calidad 1 a 3]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Grupo de Calidad 1 a 3] NOT IN ('s/d', ' ', '') and [Grupo de Calidad 1 a 3] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Roya foliar (Anaranjada)') AtributoId
	, Valor = [Roya foliar (Anaranjada) ]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Roya foliar (Anaranjada) ] NOT IN ('s/d', ' ', '') and [Roya foliar (Anaranjada) ] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Fussarium de espiga') AtributoId
	, Valor = [Fussarium de espiga]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Fussarium de espiga] NOT IN ('s/d', ' ', '') and [Fussarium de espiga] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Septoria') AtributoId
	, Valor = [Septoria]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Septoria] NOT IN ('s/d', ' ', '') and [Septoria] is not null

INSERT INTO ProductoAtributos (ProductoId, AtributoId, Valor)
Select
	p.Id ProductoId
	, (Select Id From Atributos Where Nombre = 'Dreschlera (mancha amarilla)') AtributoId
	, Valor = [Dreschlera ( mancha amarilla)]
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Where [Dreschlera ( mancha amarilla)] NOT IN ('s/d', ' ', '') and [Dreschlera ( mancha amarilla)] is not null

/* LUGARES */
INSERT INTO dbo.ProductoLugares (ProductoId, LugarId)
--Bs As SUDESTE
Select 
	p.Id ProductoId
	, l.Id LugarId
From Productos p
Inner Join Categorias c on p.CategoriaId = c.Id
Inner Join Empresas e on p.EmpresaId = e.Id
Inner Join OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
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
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\Development\AgroEnsayos\docs\Trigo.xls', 
                'SELECT * FROM [Sheet1$]') t on t.Subcategoria = c.Nombre and t.empresa = e.Nombre and t.Material = p.Material
Inner Join (
	Select Id, Region From Lugares Where Departamento is null) l on l.Region = 'SAN LUIS' AND t.SL = 'S'
	
/*
Delete From ProductoAtributos
Delete From ProductoLugares
Delete From Productos
Delete From AtributoEquivalencias
DBCC CHECKIDENT('Productos', RESEED, 0) 
*/


                