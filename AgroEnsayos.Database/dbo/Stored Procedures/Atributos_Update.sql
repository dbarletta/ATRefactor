
CREATE PROCEDURE [dbo].[Atributos_Update]
	@Id int,
	@Rubro varchar(50) = null,
	@Nombre varchar(50),
	@TipoDato tinyint,
	@Tags varchar(200) = null,
	@UsarComoFiltro bit,
	@Deshabilitado bit = 0
AS
BEGIN
	UPDATE [Attributes] 
	SET [Family] = @Rubro
	  , [Name] = @Nombre
	  , [DataType] = @TipoDato
	  , Tags = @Tags
	  , [IsFilter] = @UsarComoFiltro
	  , [IsDisabled] = 0
	WHERE Id = @Id
END
