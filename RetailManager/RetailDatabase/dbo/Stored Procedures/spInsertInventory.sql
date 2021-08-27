CREATE PROCEDURE [dbo].[spInsertInventory]
	@ProductId int, 
	@Quantity int,
	@PurchasePrice money, 
	@PurchaseDate datetime2
AS
BEGIN
	set nocount on;
	INSERT into dbo.Inventory (ProductId, Quantity, PurchasePrice, PurchaseDate)
	VALUES (@ProductId, @Quantity, @PurchasePrice, @PurchaseDate);
END
