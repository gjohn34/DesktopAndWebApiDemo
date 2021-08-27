CREATE PROCEDURE [dbo].[spGetAllInventory]
AS
BEGIN
	set nocount on;
	SELECT * FROM dbo.Inventory;
END
