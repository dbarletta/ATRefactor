CREATE PROCEDURE [dbo].[Productos_Insert] 
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
@FechaCarga date,
@Deshabilitado bit,
@ID int output
AS
BEGIN
INSERT INTO [dbo].[Productos]
           ([CategoriaId]
           ,[EmpresaId]
           ,[Nombre]
           ,[DescripcionPG]
           ,[Material]
           ,[EsHibrido]
           ,[Ciclo]
           ,[EsConvencional]
           ,[DiasFloracion]
           ,[DiasMadurez]
           ,[AlturaPlanta]
           ,[EsNuevo]
           ,[Alta]
           ,[FechaCarga]
           ,[Deshabilitado])
     VALUES(
@CategoriaId,
@EmpresaId,
@Nombre,
@DescripcionPG,
@Material,
@EsHibrido,
@Ciclo,
@EsConvencional,
@DiasFloracion, 
@DiasMadurez,
@AlturaPlanta,
@EsNuevo,
@Alta,
@FechaCarga,
@Deshabilitado 
)SET @ID = SCOPE_IDENTITY()
END