--CW July 5 2017 - add functions that detects whether an index exists

IF dbo.fnUdfExists('fnIndexExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnIndexExists]
END
GO

CREATE FUNCTION [dbo].[fnIndexExists]
(
	@name nvarchar(100),
	@tableName nvarchar(100)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM SYS.INDEXES
	WHERE NAME=@name AND object_id = OBJECT_ID(@tableName)    

	RETURN @exists
END
GO
