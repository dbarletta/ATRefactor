CREATE PROCEDURE [dbo].[AtributoCategorias_insert]
	@CategoriaId int,
	@Id int
AS
BEGIN
	 	INSERT INTO AtributoCategorias VALUES(@CategoriaId, @Id)
END