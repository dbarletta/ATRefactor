--Productos_Get 4
CREATE PROCEDURE [dbo].[Productos_TraerHabilitados]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.Id, p.[CategoryId], c.[Name] Categoria, p.[CompanyId], e.[Name] Empresa, p.[Name], p.[Description], p.Material, p.[IsHybrid], p.[Cycle], p.[IsConventional], p.[DaysToFlowering], p.[DaysToMaturity], p.[PlantHeight], p.[IsNew], p.[Height], p.[EntryDate]
	From [Products] p
	Inner Join [Companies] e on p.[CompanyId] = e.Id
	Inner Join [Categories] c on p.[CategoryId] = c.Id
	Where 
		 p.[IsDisabled] = 0
END