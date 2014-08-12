--Set Soja Tag
update temp_Ensayos set Tags = 'Soja' where Tags is null


--Sin Rinde o Rinde Nulo: 889 rows
select *
from temp_Ensayos e
where (e.Rinde is null or e.Rinde = '' or e.Rinde = 0)
and e.Tags = 'Soja'

--No cruzan con Campaña: 888 rows
select  e.Campaña, e.Producto, e.Provincia, e.Localidad, e.Establecimiento 
FROM temp_Ensayos e 
Left Join Campanas  c on e.[Campaña] = c.Nombre and c.CategoriaId = (select Id from Categorias where Nombre = 'Maiz')
where c.Id is null
and e.Tags = 'Soja'

--No cruzan con Producto: 11350 rows
select  e.Campaña, e.Producto, e.Provincia, e.Localidad, e.Establecimiento 
from temp_Ensayos e 
left join Productos p on e.Producto = p.Nombre
where p.Id is null
and e.Tags = 'Soja'

--No cruzan con Provincia: 975 rows
select  e.Campaña, e.Producto, e.Provincia, e.Localidad, e.Establecimiento 
from temp_Ensayos e 
left join Lugares l on e.Provincia = l.Provincia
where l.Id is null
and e.Tags = 'Soja'

--No cruzan con Lugar: 2801 rows
select  e.Campaña, e.Producto, e.Provincia, e.Localidad, e.Establecimiento 
from temp_Ensayos e 
left join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null and l.Id not in (1708)
left join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.localidad is null
where l.Id is null
and e.Tags = 'Soja'

--Testing Productos ordenadas por cantidad de row erroneos (correccion manual de los mas importantes)
select  e.Producto, COUNT(*)
from temp_Ensayos e 
left join Productos p on e.Producto = p.Nombre
where p.Id is null
and e.Tags = 'Soja'
group by e.Producto
order by COUNT(*) desc

--Testing Localidades ordenadas por cantidad de row erroneos (correccion manual de los mas importantes)
select  e.Provincia, e.Localidad, COUNT(*)
from temp_Ensayos e 
left join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad
where l.Id is null
and e.Tags = 'Soja'
group by e.Provincia, e.Localidad
order by COUNT(*) desc 

select distinct Localidad from Lugares where Provincia = 'cordoba' and Localidad like '%elcano%'

--Common fixes
update temp_Ensayos set Provincia = 'CORDOBA' where Provincia = 'Córdoba'
update temp_Ensayos set Localidad = 'JUAREZ, MARCOS' where Provincia = 'CORDOBA' and Localidad = 'Marco Juarez'
update temp_Ensayos set Localidad = 'JUAREZ, MARCOS' where Provincia = 'CORDOBA' and Localidad = 'Marcos Juarez'
update temp_Ensayos set Localidad = 'GENERAL JUAN MADARIAGA ,EST.G.MADARIAGA' where Provincia = 'Buenos Aires' and Localidad = 'Madariaga'
update temp_Ensayos set Localidad = 'FERRE' where Provincia = 'Buenos Aires' and Localidad = 'Ferré'
update temp_Ensayos set Localidad = 'GENERAL BALDISSERA' where Provincia = 'CORDOBA' and Localidad = 'Gral. Baldissera'
update temp_Ensayos set Localidad = 'MONTE DE LOS GAUCHOS' where Provincia = 'CORDOBA' and Localidad = 'M. de los Gauchos'
update temp_Ensayos set Localidad = 'CORONEL PRINGLES ,EST.PRINGLES' where Provincia = 'Buenos Aires' and Localidad = 'Coronel Pringles'
update temp_Ensayos set Localidad = 'CORONEL PRINGLES ,EST.PRINGLES' where Provincia = 'Buenos Aires' and Localidad = 'Coronel Pringles'
update temp_Ensayos set Localidad = 'OCAMPO, MANUEL' where Provincia = 'Buenos Aires' and Localidad = 'Manuel Ocampo'
update temp_Ensayos set Localidad = '9 DE JULIO' where Provincia = 'Buenos Aires' and Localidad = 'Nueve de Julio'
update temp_Ensayos set Localidad = 'SAN FRANCISCO DE BELLOCQ' where Provincia = 'Buenos Aires' and Localidad = 'Bellocq'
update temp_Ensayos set Localidad = 'ELCANO, SEBASTIAN' where Provincia = 'CORDOBA' and Localidad = 'Sebastian Elcano'

--Insert Into Ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)

--Soja: 1209/12608
INSERT INTO Ensayos (CampanaId, ProductoId, LugarId, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo)
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
FROM temp_Ensayos e
Inner Join Campanas  c on e.[Campaña] = c.Nombre and c.CategoriaId = (select Id from Categorias where Nombre = 'Maiz')
Left Join Productos p on e.Producto = p.Nombre
Left Join Lugares l on e.Provincia = l.Provincia and e.Localidad = l.Localidad and l.localidad is not null
Left Join Lugares l2 on l.Id is null and e.Provincia = l2.Provincia and e.Localidad = l2.Departamento and l2.Departamento is not null and l2.localidad is null
Where e.Rinde is not null and e.Rinde <> '' and e.Rinde <> 0 
  and p.Id is not null
  and (l.Id is not null or l2.Id is not null)
  and e.Tags = 'Soja'

 
 


