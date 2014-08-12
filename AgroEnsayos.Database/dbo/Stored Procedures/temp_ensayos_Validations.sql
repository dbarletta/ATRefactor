CREATE PROCEDURE [dbo].[temp_ensayos_Validations]
@categoriaId INT
AS
BEGIN
	--Datos Obligatarios
    select distinct
		1 TipoError
		, 2 Severity
		, 'Alguno de los datos obligatorios está faltando (Fuente, Provincia, Localidad, Campaña, Producto, Fecha de siembra o Rinde)' Description
		, Fuente Param1
		, Campana Param2
		, Producto Param3
		, Rinde Param4
		, Provincia + ' ' + Localidad Param5
		, Row 
	from temp_ensayos 
	where 
		Campana is null or Campana = '' or Campana = ' '  or Campana = 's/d'  
		or Producto is null or Producto = '' or Producto = ' '  or Producto = 's/d'  
		or Rinde is null or Rinde = 0  
		or Fuente is null or Fuente = '' or Fuente = ' ' or Fuente = 's/d'
		or Provincia is null or Provincia = '' or Provincia = ' ' or Provincia = 's/d'
		or Localidad is null or Localidad = '' or Localidad = ' ' or Localidad = 's/d'
		or FechaSiembra is null
	UNION
	
	--Productos
	select distinct
		2 TipoError
		, 2 Severity
		, 'El Producto ' + e.Producto + ' no se encontró en la Aplicación' Description
		, e.Producto Param1
		, '' Param2
		, '' Param3
		, '' Param4
		, '' Param5
		, Row 
	from temp_ensayos e
	Left Join Productos p on RTRIM(LTRIM(e.Producto)) = p.Nombre
	Where p.Id is null and e.Producto is not null and e.Producto != '' and e.Producto != ' ' and e.Producto != 's/d'
	
	UNION
	
	--Campaña
	select distinct
		2 TipoError
		, 2 Severity
		, 'La Campaña ' + e.Campana + ' de ' + (Select Nombre From Categorias Where Id = @categoriaId) + ' no se encontró en la Aplicación' Description
		, e.Campana Param1
		, '' Param2
		, '' Param3
		, '' Param4
		, '' Param5
		, Row 
	from temp_ensayos e
	Left Join Campanas camp on camp.Nombre = e.Campana and camp.CategoriaId = @categoriaId
	Where camp.Id is null and e.Campana is not null and e.Campana != '' and e.Campana != ' ' and e.Campana != 's/d'
	
	UNION 
	
	--Lugar
	Select distinct * From 
	(
	select distinct
		2 TipoError
		, 2 Severity
		, 'El Lugar compuesto por la Provincia de ' + e.Provincia + ' y en la Localidad / Departamento de ' + e.Localidad + ' no se encontró en la Aplicación' Description
		, e.Provincia Param1
		, e.Localidad Param2
		, CASE WHEN l.Id is not null THEN CAST(l.Id AS VARCHAR(10)) ELSE CAST(l2.Id AS VARCHAR(10)) END AS Param3
		, '' Param4
		, '' Param5
		, Row 
	from temp_ensayos e
	Left Join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null
	Left Join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.Localidad is null
	Where 
		(l.Id is null or l2.Id is null) 
		and e.Provincia is not null and e.Provincia != '' and e.Provincia != ' ' and e.Provincia != 's/d'
		and e.Localidad is not null and e.Localidad != '' and e.Localidad != ' ' and e.Localidad != 's/d'
	) data
	Where Param3 is null
	
	--Repetidos con distinto Rinde (en muchos casos tienen distinta fecha de siembra) Hay que ver con Martin que hacemos en estos casos.
	UNION
	
	Select 
		3 TipoError
		, 2 Severity
		, 'Este dato se encuentra repetido' Description
		, Fuente Param1
		, Campana Param2
		, Provincia Param3
		, Localidad Param4
		, Producto Param5
		, Repetidos  
	From (
		select Fuente, Campana, Provincia, Localidad, Producto, COUNT(Fuente) Repetidos From (
		select Fuente, Campana, Provincia, Localidad, Producto, Rinde From temp_ensayos
		where 
		Campana is null or Campana = '' or Campana = ' '  or Campana = 's/d'  
		or Producto is null or Producto = '' or Producto = ' '  or Producto = 's/d'  
		or Rinde is null or Rinde = 0  
		or Fuente is null or Fuente = '' or Fuente = ' ' or Fuente = 's/d'
		or Provincia is null or Provincia = '' or Provincia = ' ' or Provincia = 's/d'
		or Localidad is null or Localidad = '' or Localidad = ' ' or Localidad = 's/d'

		Group By Fuente, Campana, Provincia, Localidad, Producto, Rinde) data
		Group By Fuente, Campana, Provincia, Localidad, Producto
		Having Count(Fuente) > 1
	) data
		
	
	
END

