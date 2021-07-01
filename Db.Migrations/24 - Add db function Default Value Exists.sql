--CW Nov 13 17 - add functions that detects whether a default value constraint exists.
-- SEE SO: https://stackoverflow.com/a/15625778/660223


IF dbo.fnUdfExists('fnDefaultValueExists') = 1
BEGIN
	DROP FUNCTION [dbo].fnDefaultValueExists
END
GO

CREATE FUNCTION [dbo].fnDefaultValueExists
(
	@name nvarchar(100)	
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	IF OBJECT_ID(@name, 'D') IS NOT NULL SET @exists = 1;

	RETURN @exists
END
GO
