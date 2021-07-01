--CW case 2688  add function that detects whether a column has a certain datatype
--assumes this column exists
--some of the choices for datatype are:

--bit
--date
--datetime
--decimal
--float
--int
--nchar
--ntext
--numeric
--nvarchar
--timestamp
--varbinary

IF dbo.fnUdfExists('fnColumnHasDataType') = 1
BEGIN
	DROP FUNCTION [dbo].[fnColumnHasDataType]
END
GO

CREATE FUNCTION [dbo].[fnColumnHasDataType]
(
	@tableName nvarchar(100),
	@columnName nvarchar(100),
	@dataType nvarchar(50)	
)
RETURNS bit
AS
BEGIN
	declare @exists bit;

	SELECT  @exists = COUNT(*) 
	FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = @tableName
	AND COLUMN_NAME = @columnName 
	AND DATA_TYPE = @dataType

	RETURN @exists
END
GO