if not exists (select 1 from Roles where RoleName ='AcePumpAdmin')
begin
	insert into Roles (RoleName, DisplayText) values ('AcePumpAdmin', 'Full access to manage customers, invoices, etc.')
end
go