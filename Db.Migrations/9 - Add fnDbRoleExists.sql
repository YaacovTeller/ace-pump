if dbo.fnUdfExists('fnDbRoleExists') = 1
begin
	drop function dbo.fnDbRoleExists;
end
go

create function dbo.fnDbRoleExists
(
	@name nvarchar(50)
)
returns bit
as
begin
	declare @exists bit;
	select @exists = case when DATABASE_PRINCIPAL_ID(@name) is null then 0 else 1 end;

	return @exists;
end