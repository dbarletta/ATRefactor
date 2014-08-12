
--Empresas_Get 0
CREATE PROCEDURE [dbo].[Empresas_Get]
	@empresaId INT 
	,@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

		SELECT *
		From Empresas 
		Where 
			Deshabilitada = 0 
			AND (@empresaId = 0 OR (@empresaId != 0 AND Id=@empresaId))
			AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select EmpresaId from Productos where CategoriaId=@categoriaId)))
		Order by Nombre
	
END

