--Productos_Get 4
CREATE PROCEDURE [dbo].[Productos_Get]
	@categoriaId INT = NULL
	,@productoId INT = NULL
	, @todos BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.CategoriaId, c.Nombre Categoria, p.EmpresaId, e.Nombre Empresa, p.Nombre, p.DescripcionPG, p.Material, p.EsHibrido, p.Ciclo, p.EsConvencional, p.DiasFloracion, p.DiasMadurez, p.AlturaPlanta, p.EsNuevo, p.Alta, p.FechaCarga 
	From Productos p
	Inner Join Empresas e on p.EmpresaId = e.Id
	Inner Join Categorias c on p.CategoriaId = c.Id
	Where 
		(@categoriaId IS NULL OR @categoriaId = 0 OR (@categoriaId IS NOT NULL AND p.CategoriaId = @categoriaId))
		AND (@todos = 1 OR (@todos = 0 AND Deshabilitado = 0))
		AND (@productoId IS NULL OR @productoId = 0 OR (@productoId IS NOT NULL AND p.Id = @productoId))
	
END
