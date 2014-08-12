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

UPDATE Productos SET
CategoriaId = @CategoriaId,
EmpresaId = @EmpresaId,           
Nombre = @Nombre,
DescripcionPG = @DescripcionPG,
Material = @Material,
EsHibrido = @EsHibrido,
Ciclo = @Ciclo,
EsConvencional = @EsConvencional,
DiasFloracion = @DiasFloracion, 
DiasMadurez = @DiasMadurez,
AlturaPlanta = @AlturaPlanta,
EsNuevo = @EsNuevo,
Alta = @Alta,
Deshabilitado = @Deshabilitado 
 where Id = @Id
END