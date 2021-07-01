if dbo.fnColumnExists('Wells', 'Inactive') = 0
begin
   alter table Wells add Inactive bit NULL;
   update wells set inactive = 0
   ALTER TABLE Wells ALTER COLUMN Inactive BIT not null
end
go

