CREATE PROCEDURE [dbo].[Lugares_GetAllRegiones]
AS
BEGIN
	SET NOCOUNT ON;

	select Id, Region from Lugares
  where region is not null and Departamento is null and Cabecera is null
END


