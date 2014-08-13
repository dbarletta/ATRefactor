CREATE PROCEDURE [dbo].[ProductosLugares_Get]
	@productoId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	select p.Id ProductoId, l.Id LugarId, l.Region, p.[Name] ProductoNombre  from [ProductPlace]
  inner join [Products] p on p.Id = [ProductId]
  inner join [Places] l on l.Id = [PlaceId]
  where p.Id = @productoId and Region is not null and [Department] is null and [Header] is null
END

