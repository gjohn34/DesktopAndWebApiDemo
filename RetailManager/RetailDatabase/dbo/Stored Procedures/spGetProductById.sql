CREATE PROCEDURE dbo.spGetProductById
	@Id int
AS
BEGIN
	set nocount on;
	SELECT * FROM dbo.Product WHERE Id = @Id;
END
