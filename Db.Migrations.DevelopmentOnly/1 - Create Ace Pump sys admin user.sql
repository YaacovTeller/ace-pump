use master;
IF NOT EXISTS 
    (SELECT name  
     FROM master.sys.server_principals
     WHERE name = 'AcePump_Debug_User')
BEGIN
	CREATE LOGIN [AcePump_Debug_User] WITH PASSWORD = N'AcePump';
	EXECUTE sp_addsrvrolemember [AcePump_Debug_User], [sysadmin];
END
