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
		, ROUND(t.Rinde,0) Rinde
		, UPPER(t.Fuente) Fuente
		, t.Establecimiento
		, t.FechaSiembra
		, t.FechaCosecha
		, t.Indice
		, t.Observaciones
		, t.Archivo
	from temp_ensayos t
	inner join Productos prod on prod.Nombre = LTRIM(RTRIM(t.Producto))
	inner join Campanas camp on camp.Nombre = t.Campana and camp.CategoriaId = @categoriaId
	Left Join Lugares l on t.Provincia = l.Provincia and t.Localidad = l.Localidad and l.localidad is not null
	Left Join Lugares l2 on l.Id is null and t.Provincia = l2.Provincia and t.Localidad = l2.Departamento and l2.Departamento is not null and l2.Localidad is null
) as Tabla1
Where 
	LugarId is not null and Rinde is not null and Fuente is not null and FechaSiembra is not null and CampanaId is not null and ProductoId is not null 

--Update
Update Ensayos
Set Rinde = e2.Rinde, Establecimiento = e2.Establecimiento, FechaSiembra = e2.FechaSiembra, FechaCosecha = e2.FechaCosecha, Indice = e2.Indice, Archivo = e2.Archivo
From Ensayos e1
Inner Join @ensayos e2 on e1.CampanaId = e2.CampanaId and e1.ProductoId = e2.ProductoId and e1.LugarId = e2.LugarId and e1.Fuente = e2.Fuente and ISNULL(e1.Observaciones,'1') = ISNULL(e2.Observaciones,'1')
Where 
	e2.Rinde != e1.Rinde
	OR e2.Establecimiento != e1.Establecimiento
	OR e2.FechaSiembra != e1.FechaSiembra
	OR e2.FechaCosecha != e1.FechaCosecha
	OR e2.Indice != e1.Indice
	OR e2.Archivo != e1.Archivo
	OR e2.Observaciones != e1.Observaciones
	
--Insert	
INSERT INTO Ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)
Select e1.CampanaId, e1.ProductoId, e1.LugarId, e1.Rinde, e1.Fuente, e1.Establecimiento, e1.FechaSiembra, e1.FechaCosecha, e1.Indice, e1.Observaciones, e1.Archivo From @ensayos e1
Left Join Ensayos e2 on e1.CampanaId = e2.CampanaId and e1.ProductoId = e2.ProductoId and e1.LugarId = e2.LugarId and e1.Fuente = e2.Fuente and ISNULL(e1.Observaciones,'1') = ISNULL(e2.Observaciones,'1')
Where e2.Id is null

--Borro Tabla temp_ensayos
Truncate Table temp_ensayos


END