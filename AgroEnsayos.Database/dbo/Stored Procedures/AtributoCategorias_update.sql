CREATE PROCEDURE [dbo].[AtributoCategorias_update]
	@CategoriaId int,
	@Id int
AS
BEGIN
	 	Update AtributoCategorias set CategoriaId = @CategoriaId where AtributoId = @Id
END