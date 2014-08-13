--Productos_Get 4
CREATE PROCEDURE [dbo].[Productos_Get]
	@categoriaId INT = NULL
	,@productoId INT = NULL
	, @todos BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.[CategoryId], c.[Name] Categoria, p.[CompanyId], e.[Name] Empresa, p.[Name], p.[Description], p.Material, p.[IsHybrid], p.[Cycle], p.[IsConventional], p.[DaysToFlowering], p.[DaysToMaturity], p.[PlantHeight], p.[IsNew], p.[Height], p.[EntryDate] 
	From [Products] p
	Inner Join [Companies] e on p.[CompanyId] = e.Id
	Inner Join [Categories] c on p.[CategoryId] = c.Id
	Where 
		(@categoriaId IS NULL OR @categoriaId = 0 OR (@categoriaId IS NOT NULL AND p.[CategoryId] = @categoriaId))
		AND (@todos = 1 OR (@todos = 0 AND p.[IsDisabled] = 0))
		AND (@productoId IS NULL OR @productoId = 0 OR (@productoId IS NOT NULL AND p.Id = @productoId))
	
END
