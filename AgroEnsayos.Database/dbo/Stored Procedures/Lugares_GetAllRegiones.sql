CREATE PROCEDURE [dbo].[Lugares_GetAllRegiones]
AS
BEGIN
	SET NOCOUNT ON;

	select Id, Region from [Places]
  where region is not null and [Department] is null and [Header] is null
END


