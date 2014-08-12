/* TRIGO */
/*
Insert Into Ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)
Select CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo From (
Select distinct 
	c.Id CampanaId
	, p.Id ProductoId
	, CASE WHEN l.Id is not null THEN l.Id ELSE l2.Id END AS LugarId
	, e.Rinde
	, e.Fuente
	, e.[Establecimiento]
	, e.[Fecha Siembra] FechaSiembra
	, e.[Fecha Cosecha] FechaCosecha
	, e.[Indice %] Indice
	, e.Observaciones
	, e.[Archivo Asociado] Archivo
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\AgroEnsayos-Bases\Ensayos Trigo.xlsx', 
                'SELECT * FROM [Sheet1$]') e 
                Inner Join Campanas  c on e.[Campaña] = c.Nombre and c.CategoriaId = 4
                Inner Join Productos p on e.Producto = p.Nombre
                Left Join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null and l.Id not in (1708)
                Left Join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.localidad is null
Where Rinde is not null
) data 
*/

/* MAIZ */

Delete From ProductoAtributos Where ProductoId in (Select Id From Productos Where CategoriaId = 3)
Delete From ProductoLugares Where ProductoId in (Select Id From Productos Where CategoriaId = 3)
Delete From Productos where CategoriaId = 3

DBCC CHECKIDENT('Productos', RESEED, 123) 

Empresas
Ensayos

Insert Into Ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)
Select CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo From (
Select distinct 
	c.Id CampanaId
	, p.Id ProductoId
	, CASE WHEN l.Id is not null THEN l.Id ELSE l2.Id END AS LugarId
	, e.Rinde
	, e.Fuente
	, e.[Establecimiento]
	, e.[Fecha Siembra] FechaSiembra
	, e.[Fecha Cosecha] FechaCosecha
	, e.[Indice %] Indice
	, e.Observaciones
	, e.[Archivo Asociado] Archivo
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\AgroEnsayos-Bases\Ensayos Maiz.xlsx', 
                'SELECT * FROM [Sheet1$]') e 
                Inner Join Campanas  c on e.[Campaña] = c.Nombre and c.CategoriaId = 3
                Left Join Productos p on e.Producto = p.Nombre
                Left Join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null and l.Id not in (1708)
                Left Join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.localidad is null
Where Rinde is not null and p.Id is not null
) data 




Select distinct 
	
	e.Producto
	
FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\AgroEnsayos-Bases\Ensayos Maiz.xlsx', 
                'SELECT * FROM [Sheet1$]') e 
                Inner Join Campanas  c on e.[Campaña] = c.Nombre and c.CategoriaId = 3
                Left Join Productos p on e.Producto = p.Nombre
                Left Join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null and l.Id not in (1708)
                Left Join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.localidad is null
Where Rinde is not null and p.Id is null

/*

Select * From Lugares where provincia = 'Cordoba' and localidad = 'El Quebracho'

Select * From Lugares l
where id in (
Select l.id From Lugares l inner join (
Select provincia, localidad From Lugares where localidad is not null group by provincia, localidad having count(*) > 1) r on l.provincia = r.provincia and l.localidad = r.localidad)
order by l.provincia, l.localidad, l.departamento
--Inner Join Productos p on e.Producto = p.Nombre
*/

/*
INSERT INTO Lugares (Region, Provincia, Departamento, Localidad)
Select distinct l.Region, x.Provincia, x.Departamento, x.Localidad FROM OPENROWSET('Microsoft.ACE.OLEDB.12.0',
                'Excel 8.0;HDR=Yes;IMEX=1;Database=C:\Users\lucasg\Dropbox\AgroEnsayos-Bases\Lugares.xlsx', 
                'SELECT * FROM [localidades$]') x
Inner Join Lugares l on x.Provincia = l.Provincia and x.Departamento = l.Departamento
*/


/*
Update Lugares
Set Departamento = 'CAÑUELAS', Cabecera = 'CAÑUELAS'
Where Id = 469

Update Lugares
Set Departamento = 'PATIÑO'
Where Id = 164

Update Lugares
Set Departamento = 'GENERAL ANGEL V. PEÑALOZA'
Where Id = 61

Update Lugares
Set Departamento = 'ROSARIO VERA PEÑALOZA'
Where Id = 189

Update Lugares
Set Departamento = 'LA VIÑA'
Where Id = 176

*/

