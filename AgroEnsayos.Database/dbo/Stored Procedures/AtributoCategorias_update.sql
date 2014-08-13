CREATE PROCEDURE [dbo].[AtributoCategorias_update]
	@CategoriaId int,
	@Id int
AS
BEGIN
	 	Update [AttributeCategory] set [CategoryId] = @CategoriaId where [AttributeId] = @Id
END