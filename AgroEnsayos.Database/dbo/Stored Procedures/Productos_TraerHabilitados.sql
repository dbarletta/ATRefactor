--Productos_Get 4
CREATE PROCEDURE [dbo].[Productos_TraerHabilitados]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.CategoriaId, c.Nombre Categoria, p.EmpresaId, e.Nombre Empresa, p.Nombre, p.DescripcionPG, p.Material, p.EsHibrido, p.Ciclo, p.EsConvencional, p.DiasFloracion, p.DiasMadurez, p.AlturaPlanta, p.EsNuevo, p.Alta, p.FechaCarga
	From Productos p
	Inner Join Empresas e on p.EmpresaId = e.Id
	Inner Join Categorias c on p.CategoriaId = c.Id
	Where 
		 Deshabilitado = 0
END