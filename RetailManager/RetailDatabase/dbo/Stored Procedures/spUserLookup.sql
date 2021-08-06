CREATE PROCEDURE [dbo].[spUserLookup]
	@Id nvarchar(128)
AS
BEGIN
	set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	from [dbo].[User]
	WHERE Id = @Id;
END
RETURN 0