if dbo.fnColumnExists('Customers', 'PayUpFront') = 0
begin
   alter table Customers add PayUpFront bit NULL;
end
go
