IF dbo.fnTableExists('Log_HttpRequest') = 0
BEGIN
	CREATE TABLE [dbo].[Log_HttpRequest](
		[Log_HttpRequestID] [int] IDENTITY(1,1) PRIMARY KEY	,
		[Environment] [nvarchar](max) NULL,
		[Path] [nvarchar](max) NULL,
		[RequestTime] [datetime] NOT NULL,
		[HttpMethod] [nvarchar](max) NULL,
		[LoggedInUsername] [nvarchar](max) NULL
	 );

	 CREATE TABLE [dbo].[Log_HttpRequestParam](
		[Log_HttpRequestParamID] [int] IDENTITY(1,1) PRIMARY KEY,
		[Log_HttpRequestID] [int] NOT NULL,
		[ParamType] [nvarchar](max) NULL,
		[ParamName] [nvarchar](max) NULL,
		[ParamValue] [nvarchar](max) NULL
	);

	ALTER TABLE [dbo].[Log_HttpRequestParam]  ADD CONSTRAINT [FK_dbo.Log_HttpRequestParam_dbo.Log_HttpRequest_Log_HttpRequestID] FOREIGN KEY([Log_HttpRequestID])
	REFERENCES [dbo].[Log_HttpRequest] ([Log_HttpRequestID])
	ON DELETE CASCADE
END
GO