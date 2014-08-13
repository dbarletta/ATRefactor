CREATE PROCEDURE [dbo].[Productos_Edit] 
@CategoriaId int,
@EmpresaId int,
@Nombre varchar(50),
@DescripcionPG varchar(300),
@Material varchar(50),
@EsHibrido bit,
@Ciclo varchar(50),
@EsConvencional bit,
@DiasFloracion int,
@DiasMadurez int,
@AlturaPlanta int,
@EsNuevo bit,
@Alta int,
@Deshabilitado bit,
@Id int
AS
BEGIN

UPDATE [Products] SET
[CategoryId] = @CategoriaId,
[CompanyId] = @EmpresaId,           
[Name] = @Nombre,
[Description] = @DescripcionPG,
Material = @Material,
[IsHybrid] = @EsHibrido,
[Cycle] = @Ciclo,
[IsConventional] = @EsConvencional,
[DaysToFlowering] = @DiasFloracion, 
[DaysToMaturity] = @DiasMadurez,
[PlantHeight] = @AlturaPlanta,
[IsNew] = @EsNuevo,
[Height] = @Alta,
[IsDisabled] = @Deshabilitado 
 where Id = @Id
END