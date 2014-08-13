
CREATE PROCEDURE [dbo].[ProductoLugares_Edit]
@productid int,
@lugarid int
AS
BEGIN
	SET NOCOUNT ON;
insert into [ProductPlace] ([ProductId], [PlaceId]) values (@productid, @lugarid)

END

