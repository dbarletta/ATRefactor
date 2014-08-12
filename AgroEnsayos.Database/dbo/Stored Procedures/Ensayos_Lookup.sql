
CREATE PROCEDURE [dbo].[Ensayos_Lookup]
	@categoriaId INT
	, @searchTerm VARCHAR(100) = ''
	, @empresa VARCHAR(100)
	, @fuente VARCHAR(100)
	, @provincia VARCHAR(200)
	, @localidad VARCHAR(200)
	, @campana VARCHAR(200)
	, @atributoTbl AtributoEquivalenciasType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ranking TABLE (Id INT, Ranking INT)
	INSERT INTO @ranking (Id, Ranking)
	Select en.Id, RANK() OVER (PARTITION BY en.LugarId, en.CampanaId, en.Fuente ORDER BY en.Rinde DESC) From Ensayos en
	Inner Join Productos p on en.ProductoId = p.Id
	Where p.CategoriaId = @categoriaId
	
	DECLARE @total TABLE (LugarId INT, CampanaId INT, Fuente VARCHAR(50), Total INT)
	INSERT INTO @total (LugarId, CampanaId, Fuente, Total)
	Select en.LugarId, en.CampanaId, en.Fuente, COUNT(LugarId) Total From Ensayos en
	Inner Join Productos p on en.ProductoId = p.Id
	Where p.CategoriaId = @categoriaId
	Group By LugarId, CampanaId, Fuente 
	
	SELECT en.Id, p.CategoriaId, c.Nombre Categoria, en.Fuente
		, e.Nombre Empresa, p.Nombre Producto, p.DescripcionPG
		, en.CampanaId , camp.Nombre Campana, en.Rinde , en.Establecimiento , en.FechaCosecha
		, en.FechaSiembra , en.Indice , en.ProductoId , en.Observaciones , en.LugarId , l.Departamento
		, en.Archivo, l.Provincia , ISNULL(l.Localidad,l.Departamento) localidad  
		, r.Ranking
		, t.Total
	From Ensayos en
	Inner Join Productos p on en.ProductoId = p.Id
	Inner Join Lugares l on en.LugarId = l.Id
	Inner Join Empresas e on p.EmpresaId = e.Id
	Inner Join Categorias c on p.CategoriaId = c.Id
	Inner join Campanas camp on en.CampanaId = camp.Id
	Inner Join @ranking r on en.Id = r.Id
	Inner Join @total t on en.LugarId = t.LugarId and en.CampanaId = t.CampanaId and en.Fuente = t.Fuente
	left join (select distinct pa.ProductoId productoId 
				from ProductoAtributos pa 
					inner join AtributoEquivalencias ae on (pa.AtributoId = ae.AtributoId and pa.Valor = ae.Valor  ) 
					inner join @atributoTbl atbl on (atbl.AtributoId = ae.AtributoId and atbl.Equivalencia = ae.Equivalencia)
				) as atrp on ( p.Id = atrp.productoId )
	Where p.CategoriaId = @categoriaId
	AND (atrp.productoId is not null or (select COUNT(*) from @atributoTbl)=0 )
	AND (@empresa = '' OR CHARINDEX(','+cast(e.Id as varchar)+',', @empresa) > 0  )
	AND (@fuente = '' OR CHARINDEX(','+cast(en.Fuente as varchar)+',', @fuente) > 0   )
	AND (@provincia = '' OR CHARINDEX(','+cast(l.Provincia as varchar)+',', @provincia) > 0   )
	AND (@localidad = '' OR CHARINDEX(','+cast(l.Localidad as varchar)+',', @localidad) > 0   )
	AND (@campana = '' OR (@campana != '' AND en.CampanaId in (select c.Id from Campanas c where c.CategoriaId = @categoriaId and CHARINDEX(',' + cast(c.Id as VARCHAR) + ',', @campana) > 0 ))) 
	
	AND (@searchTerm = '' OR (@searchTerm != '' AND (CHARINDEX(@searchTerm, p.Nombre) > 0 
		OR CHARINDEX(@searchTerm, en.Fuente) > 0
		OR CHARINDEX(@searchTerm, e.Nombre) > 0
		OR CHARINDEX(@searchTerm, l.Localidad) > 0
		OR CHARINDEX(@searchTerm, l.Departamento) > 0
		OR p.Id in (
			Select distinct pa.ProductoId From ProductoAtributos pa
			Inner Join AtributoCategorias ac on pa.AtributoId = ac.AtributoId
			Inner Join Atributos a on ac.AtributoId = a.Id
			Where ac.CategoriaId = @categoriaId and CHARINDEX(@searchTerm, a.Tags) > 0
			UNION
			Select distinct pl.ProductoId From ProductoLugares pl
			Inner Join Productos p on pl.ProductoId = p.Id and p.CategoriaId = @categoriaId
			Inner Join Lugares l on pl.LugarId = l.Id
			Where
				CHARINDEX(@searchTerm, l.Region) > 0
				OR CHARINDEX(@searchTerm, l.Provincia) > 0
				OR CHARINDEX(@searchTerm, l.Departamento) > 0
				OR CHARINDEX(@searchTerm, l.Cabecera) > 0)))) 
				
END
