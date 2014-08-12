
--Empresas_Get 0
CREATE PROCEDURE [dbo].[Empresas_GetNombres]
AS
BEGIN
	SET NOCOUNT ON;
		SELECT Nombre From Empresas Where Deshabilitada = 0 
		Order by Nombre
END