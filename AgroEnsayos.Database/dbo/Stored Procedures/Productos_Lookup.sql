/*
	DECLARE @atributoTbl AtributoEquivalenciasType
	EXEC Productos_Lookup 4, '', '', '', 'BUENOS AIRES CENTRO', @atributoTbl
*/
CREATE PROCEDURE [dbo].[Productos_Lookup]
	@categoriaId INT
	, @searchTerm VARCHAR(100) = ''
	, @empresa VARCHAR(100)
	, @antiguedad VARCHAR(100)
	, @region VARCHAR(200)
	, @atributoTbl AtributoEquivalenciasType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.CategoriaId, c.Nombre Categoria, p.EmpresaId, e.Nombre Empresa, p.Nombre, p.DescripcionPG, p.Material, p.EsHibrido, p.Ciclo, p.EsConvencional, p.DiasFloracion, p.DiasMadurez, p.AlturaPlanta, p.EsNuevo, p.Alta, p.FechaCarga 
	From Productos p
	Inner Join Empresas e on p.EmpresaId = e.Id
	Inner Join Categorias c on p.CategoriaId = c.Id
	left join (select distinct pa.ProductoId productoId 
				from ProductoAtributos pa 
					inner join AtributoEquivalencias ae on (pa.AtributoId = ae.AtributoId and pa.Valor = ae.Valor  ) 
					inner join @atributoTbl atbl on (atbl.AtributoId = ae.AtributoId and atbl.Equivalencia = ae.Equivalencia)
				) as atrp on ( p.Id = atrp.productoId )
	Where CategoriaId = @categoriaId
	AND (e.Deshabilitada = 0) --Agregado para que no traiga productos de empresas deshabilitadas
	AND (atrp.productoId is not null or (select COUNT(*) from @atributoTbl)=0 )
	AND (@empresa = '' OR CHARINDEX(','+cast(e.Id as varchar)+',', @empresa) > 0  )
	AND (@antiguedad = '' OR CHARINDEX(','+cast(p.EsNuevo as varchar)+',', @antiguedad) > 0   )
	AND (@region = '' OR (@region != '' AND p.Id in (select pl.ProductoId from ProductoLugares pl, Lugares l where pl.LugarId = l.Id and CHARINDEX(',' + l.Region + ',', @region) > 0 and l.Departamento is null))) 
	AND (@searchTerm = '' OR (@searchTerm != '' AND (CHARINDEX(@searchTerm, p.Nombre) > 0 
		OR CHARINDEX(@searchTerm, p.Material) > 0
		OR CHARINDEX(@searchTerm, e.Nombre) > 0
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
