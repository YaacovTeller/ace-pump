-- dbo.fnUdfExists
IF EXISTS (
		   SELECT *
           FROM   sys.objects
           WHERE  object_id = OBJECT_ID(N'[dbo].[fnUdfExists]')
                  AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
BEGIN
	DROP FUNCTION [dbo].[fnUdfExists]
END
GO

CREATE FUNCTION [dbo].[fnUdfExists]
(
	@name nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = (case when count(*) > 0 then 1 else 0 end)
	FROM   sys.objects
	WHERE  object_id = OBJECT_ID(N'[dbo].[' + @name + ']')
		AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' )

	RETURN @exists
END
GO

-- dbo.fnTableExists
IF dbo.fnUdfExists('fnTableExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnTableExists]
END
GO

CREATE FUNCTION [dbo].[fnTableExists]
(
	@name nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND  TABLE_NAME = @name

	RETURN @exists
END
GO

-- dbo.fnColumnExists
IF dbo.fnUdfExists('fnColumnExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnColumnExists]
END
GO

CREATE FUNCTION [dbo].[fnColumnExists]
(
	@table nvarchar(50),
	@column nvarchar(50)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM sys.columns 
    WHERE Name      = @column
      AND Object_ID = Object_ID(@table)

	RETURN @exists
END
GO

-- dbo.fnConstraintExists
IF dbo.fnUdfExists('fnConstraintExists') = 1
BEGIN
	DROP FUNCTION [dbo].[fnConstraintExists]
END
GO

CREATE FUNCTION [dbo].[fnConstraintExists]
(
	@name nvarchar(100)
)
RETURNS bit
AS
BEGIN
	declare @exists bit;
	SELECT @exists = COUNT(*)
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE CONSTRAINT_NAME=@name

	RETURN @exists
END
GO

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


-- dbo.ClrRound_10_4
IF dbo.fnUdfExists('ClrRound_10_4') = 1
BEGIN
	DROP FUNCTION [dbo].[ClrRound_10_4]
END
GO

CREATE FUNCTION [dbo].[ClrRound_10_4]
(
	@value decimal(28, 10)
)
RETURNS decimal(10,4)
AS
BEGIN
	DECLARE @converted decimal(10,4)

	SELECT @converted = cast(round(@value, 4) as decimal(10,4))

	RETURN @converted

END
GO