-- CW case 1150 case 1503

if dbo.fnColumnExists('tblParts', 'Discount_as_Decimal') = 1
begin
		if dbo.fnDefaultValueExists('dbo.DF__tblParts__Profit__24927208') = 1
		begin
			ALTER TABLE dbo.tblParts DROP CONSTRAINT DF__tblParts__Profit__24927208;
		end		

		ALTER TABLE tblParts DROP COLUMN Discount;

        exec sp_rename 'tblParts.Discount_as_Decimal', 'Discount';
end
go

if dbo.fnColumnExists('tblParts', 'Cost_As_Decimal') = 1
begin
		ALTER TABLE tblParts DROP COLUMN Cost;
        exec sp_rename 'tblParts.Cost_As_Decimal', 'Cost';
end
go

if dbo.fnColumnExists('tblParts', 'Markup_As_Decimal') = 1
begin	
		ALTER TABLE tblParts DROP COLUMN Markup;
        exec sp_rename 'tblParts.Markup_As_Decimal', 'Markup';
end
go