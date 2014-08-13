CREATE PROCEDURE [dbo].[Atributos_Insert]
	@Rubro varchar(50) = null,
	@Nombre varchar(50),
	@TipoDato tinyint,
	@Tags varchar(200) = null,
	@UsarComoFiltro bit,
	@Deshabilitado bit = 0
AS
BEGIN
	INSERT INTO [Attributes] VALUES(@Rubro, @Nombre, @TipoDato, @Tags, @UsarComoFiltro, @Deshabilitado)
	
	SELECT SCOPE_IDENTITY()
END