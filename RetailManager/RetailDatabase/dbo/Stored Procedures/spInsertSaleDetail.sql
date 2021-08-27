CREATE PROCEDURE [dbo].[spInsertSaleDetail]
	@Id int output,
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money
AS
BEGIN
	set nocount on;
	INSERT into dbo.SaleDetail 
	(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax)

	select @Id = SCOPE_IDENTITY();
END