﻿CREATE PROCEDURE [dbo].[spGetSalesReport]

AS
BEGIN
	set nocount on;
	select [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], u.FirstName, u.LastName, u.EmailAddress 
	from dbo.Sale s
	inner join dbo.[User] u on s.UserId = u.Id;
END
