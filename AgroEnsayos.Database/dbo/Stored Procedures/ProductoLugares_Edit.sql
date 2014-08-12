
CREATE PROCEDURE [dbo].[ProductoLugares_Edit]
@productid int,
@lugarid int
AS
BEGIN
	SET NOCOUNT ON;
insert into ProductoLugares (ProductoId, LugarId) values (@productid, @lugarid)

END

