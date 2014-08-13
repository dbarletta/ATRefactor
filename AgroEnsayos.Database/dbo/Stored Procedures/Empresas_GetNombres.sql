
--Empresas_Get 0
CREATE PROCEDURE [dbo].[Empresas_GetNombres]
AS
BEGIN
	SET NOCOUNT ON;
		SELECT [Name] From [Companies] Where [IsDisabled] = 0 
		Order by [Name]
END