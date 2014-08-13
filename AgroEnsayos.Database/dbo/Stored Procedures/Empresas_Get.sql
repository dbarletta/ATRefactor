
--Empresas_Get 0
CREATE PROCEDURE [dbo].[Empresas_Get]
	@empresaId INT 
	,@categoriaId int
AS
BEGIN
	SET NOCOUNT ON;

		SELECT *
		From [Companies] 
		Where 
			[IsDisabled] = 0 
			AND (@empresaId = 0 OR (@empresaId != 0 AND Id=@empresaId))
			AND (@categoriaId = 0 OR (@categoriaId != 0 AND Id in (select [CompanyId] from [Products] where [CategoryId]=@categoriaId)))
		Order by [Name]
	
END

