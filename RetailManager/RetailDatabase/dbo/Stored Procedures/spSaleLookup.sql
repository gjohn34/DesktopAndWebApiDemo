CREATE PROCEDURE [dbo].[spSaleLookup]
	@UserId nvarchar(128),
	@SaleDate datetime2
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id
	FROM dbo.Sale
	WHERE @UserId = UserId AND @SaleDate = SaleDate;
END