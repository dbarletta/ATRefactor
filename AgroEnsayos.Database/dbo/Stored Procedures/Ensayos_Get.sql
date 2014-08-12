
--Ensayos_Get 4, null, 1, 2439, 'INASE', 1
CREATE PROCEDURE [dbo].[Ensayos_Get]
	@categoriaId INT = NULL
	, @productoId INT = NULL
	, @campanaId INT = NULL
	, @lugarId INT = NULL
	, @fuente VARCHAR(50) = NULL
	, @isChart BIT = 0
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
	
	DECLARE @ensayos TABLE (Id INT, CampanaId INT, Campana VARCHAR(50), ProductoId INT, Producto VARCHAR(50), LugarId INT, Provincia VARCHAR(50), Departamento VARCHAR(50), Localidad VARCHAR(50), Rinde SMALLMONEY, Fuente VARCHAR(50), Establecimiento VARCHAR(50), FechaSiembra DATE, FechaCosecha Date, Indice INT, Observaciones VARCHAR(100), Archivo VARCHAR(100), CategoriaId INT, Categoria VARCHAR(50), Ranking INT, Total INT)
	
	INSERT INTO @ensayos (Id, CampanaId, Campana, ProductoId, Producto, LugarId, Provincia, Departamento, Localidad, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo, CategoriaId, Categoria, Ranking, Total)
		SELECT e.Id, e.CampanaId, c.Nombre Campana, e.ProductoId, p.Nombre Producto, e.LugarId, l.Provincia, l.Departamento, l.Localidad, e.Rinde, e.Fuente, e.Establecimiento, e.FechaSiembra, e.FechaCosecha, e.Indice, e.Observaciones, e.Archivo 
				, p.CategoriaId , ca.Nombre Categoria, r.Ranking, t.Total
		From Ensayos e
		Inner Join Productos p on e.ProductoId = p.Id
		Inner Join Categorias ca on ca.Id = p.CategoriaId
		Inner Join Campanas c on e.CampanaId = c.Id
		Inner Join @ranking r on e.Id = r.Id
		Inner Join @total t on e.LugarId = t.LugarId and e.CampanaId = t.CampanaId and e.Fuente = t.Fuente
		Left Join Lugares l on e.LugarId = l.Id
		Where 
			(@categoriaId IS NULL OR @categoriaId = 0 OR (@categoriaId IS NOT NULL AND p.CategoriaId = @categoriaId))
			AND (@productoId IS NULL OR (@productoId IS NOT NULL AND e.ProductoId = @productoId))
			AND (@campanaId IS NULL OR (@campanaId IS NOT NULL AND e.CampanaId = @campanaId))
			AND (@lugarId IS NULL OR (@lugarId IS NOT NULL AND e.LugarId = @lugarId))
			AND (@fuente IS NULL OR (@fuente IS NOT NULL AND e.Fuente = @fuente))
		Order by e.Rinde DESC, e.CampanaId, e.ProductoId, l.Provincia, l.Departamento, l.Localidad
			
		IF @isChart = 0
			Select * From @ensayos
		ELSE
			Select * From @ensayos
			Where Ranking <= 5
			UNION 
			Select * From (Select TOP 1 Id, CampanaId, Campana, ProductoId, 'ULTIMO' Producto, LugarId, Provincia, Departamento, Localidad, Rinde, Fuente, Establecimiento, FechaSiembra, FechaCosecha, Indice, Observaciones, Archivo, CategoriaId, Categoria, Ranking, Total From @ensayos Order By Ranking DESC) data
			UNION
			Select 0 Id, CampanaId, Campana, 0 ProductoId, 'PROMEDIO', LugarId, Provincia, Departamento, Localidad, AVG(Rinde) Rinde, Fuente, '' Establecimiento, null FechaSiembra, null FechaCosecha, null Indice, null Observaciones, null Archivo, CategoriaId, Categoria, 0 Ranking, 0 Total
			From @ensayos
			Group By CampanaId, Campana, LugarId, Provincia, Departamento, Localidad, Fuente, CategoriaId, Categoria
			Order By Rinde DESC
	
END