CREATE PROCEDURE [dbo].[AtributoCategorias_insert]
	@CategoriaId int,
	@Id int
AS
BEGIN
	 	INSERT INTO [AttributeCategory] VALUES(@CategoriaId, @Id)
END