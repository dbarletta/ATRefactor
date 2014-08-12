CREATE PROCEDURE [dbo].[Atributo_Deshabilitar]
	@Id int
AS
	UPDATE Atributos SET Deshabilitado = 1 WHERE Id = @Id
RETURN 0
