-- This script deletes a template by ID

-- Verify merge info
declare @DeleteTemplateID int = 0,
		@MatchCount int = 0
		
select @MatchCount = count(*) from tblPumpTemplates where KPPumpTemplateID=@DeleteTemplateID
if @MatchCount<1 throw 50000 ,'Could not find template!',0

-- Verify no pumps use this template
select @MatchCount=count(*)
from tblPumps
where KFPumpTemplateID=@DeleteTemplateID
if @MatchCount>0 throw 50000 ,'Some pumps use that template!',0

-- Delete part defs
delete from tblTemplatePartsJoin where KFPumpTemplateID=@DeleteTemplateID

-- Delete template
delete from tblPumpTemplates where KPPumpTemplateID=@DeleteTemplateID