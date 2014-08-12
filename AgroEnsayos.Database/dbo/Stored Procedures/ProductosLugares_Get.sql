CREATE PROCEDURE [dbo].[ProductosLugares_Get]
	@productoId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	select p.Id ProductoId, l.Id LugarId, l.Region, p.Nombre ProductoNombre  from ProductoLugares
  inner join Productos p on p.Id = ProductoId
  inner join Lugares l on l.Id = LugarId
  where p.Id = @productoId and Region is not null and Departamento is null and Cabecera is null
END

