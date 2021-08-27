CREATE PROCEDURE [dbo].[spInsertSale]
	@Id int output,
	@UserId nvarchar(128), 
	@SaleDate datetime2, 
	@SubTotal money, 
	@Tax money, 
	@Total money
AS
BEGIN
	set nocount on;

	INSERT into dbo.Sale
	(UserId, SaleDate, SubTotal, Tax, Total)
	VALUES (@UserId, @SaleDate, @SubTotal, @Tax, @Total);

	select @Id = SCOPE_IDENTITY();
END
