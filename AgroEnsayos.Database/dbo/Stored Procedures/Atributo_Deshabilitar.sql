CREATE PROCEDURE [dbo].[Atributo_Deshabilitar]
	@Id int
AS
	UPDATE [Attributes] SET [IsDisabled] = 1 WHERE Id = @Id
RETURN 0
