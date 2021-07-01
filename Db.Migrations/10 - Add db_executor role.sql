if dbo.fnDbRoleExists('db_executor') = 0
begin
	create role db_executor;
	grant execute to db_executor;

	EXECUTE sp_addrolemember db_executor, [AcePump_IUsr];
end