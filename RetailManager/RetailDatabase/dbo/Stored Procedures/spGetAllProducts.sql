CREATE PROCEDURE [dbo].[spGetAllProducts]
AS
BEGIN
	set nocount on;

	SELECT Id, [Name], [Description], QuantityInStock, RetailPrice 
	FROM dbo.Product 
	order by Name
END
RETURN 0
