if dbo.fnConstraintExists('FK_tblPumpTemplateDefJoin_tblPumpTemplates') = 1
begin
	alter table tblPumpTemplateSpecsJoin drop constraint FK_tblPumpTemplateDefJoin_tblPumpTemplates
end

if dbo.fnTableExists('tblPumpTemplateSpecsJoin') = 1
begin
	drop table tblPumpTemplateSpecsJoin
end

if dbo.fnTableExists('tblPumpSpecs') = 1
begin
	drop table tblPumpSpecs
end