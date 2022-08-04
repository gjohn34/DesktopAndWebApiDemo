CREATE PROCEDURE [dbo].[spUpdateProductQuantity]
	@Id int,
	@Quantity int
AS
BEGIN
	set nocount on;

	UPDATE dbo.Product
	SET QuantityInStock = QuantityInStock - @Quantity
	WHERE Id = @Id;
END