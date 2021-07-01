-- This script merges two different templates by ID including moving all pumps

-- Verify merge info
declare @MergeTargetTemplateID int = 0,
		@MergeSourceTemplateID int = 0,
		@MatchCount int = 0
		
select @MatchCount = count(*) from tblPumpTemplates where KPPumpTemplateID=@MergeSourceTemplateID
if @MatchCount<1 throw 50000 ,'Could not find source template!',0
if @MatchCount>1 throw 50000 ,'Found more than one source template!',0

select @MatchCount = count(*) from tblPumpTemplates where KPPumpTemplateID=@MergeTargetTemplateID
if @MatchCount<1 throw 50000 ,'Could not find target template!',0
if @MatchCount>1 throw 50000 ,'Found more than one target template!',0

-- Verify same number and part id's of part defs
select @MatchCount=count(*)
from tblTemplatePartsJoin
where
	KFPumpTemplateID=@MergeSourceTemplateID
	and KFPartsID not in (select KFPartsID from tblTemplatePartsJoin where KFPumpTemplateID=@MergeTargetTemplateID)
if @MatchCount>0 throw 50000 ,'Parts in source that were not target!',0

select @MatchCount=count(*)
from tblTemplatePartsJoin
where
	KFPumpTemplateID=@MergeTargetTemplateID
	and KFPartsID not in (select KFPartsID from tblTemplatePartsJoin where KFPumpTemplateID=@MergeSourceTemplateID)
if @MatchCount>0 throw 50000 ,'Parts in target that were not source!',0


-- Delete source part defs
delete from tblTemplatePartsJoin where KFPumpTemplateID=@MergeSourceTemplateID

-- Move pumps
update tblPumps set KFPumpTemplateID=@MergeTargetTemplateID where KFPumpTemplateID=@MergeSourceTemplateID

-- Delete source
delete from tblPumpTemplates where KPPumpTemplateID=@MergeSourceTemplateID