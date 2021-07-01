-- This script finds all wells in the DB with the same well number / lease and merges them into a single well.

-- Load duplicate info
declare @DuplicateWells table (
	BaseID int, 
	DuplicateID int,
	LeaseID int,
	WellNumber nvarchar(max)
);

insert into @DuplicateWells 
select
	base.KPWellLocationID,
	dup.KPWellLocationID,
	base.KFLeaseID,
	base.WellNumber

from 
	tblWellLocation base
	inner join tblWellLocation dup on base.WellNumber=dup.WellNumber and base.KFLeaseID = dup.KFLeaseID and base.KPWellLocationID <> dup.KPWellLocationID

-- All dups register twice, once as the Base, once as the Dup.  Merge.
delete from @DuplicateWells
where BaseID not in (select min(BaseID) from @DuplicateWells group by LeaseID, WellNumber having count(*)>=2)

-- Move all d. tickets off of duplicate wells
select 
	dt.KPShopTicketID,
	dt.KFWellLocationID as OriginalWellID,
	dw.BaseID as MergedToWellID,
	dw.LeaseID,
	dw.WellNumber

from
	tblShopTickets dt
	inner join @DuplicateWells dw on dw.DuplicateID = dt.KFWellLocationID

update tblShopTickets
	set KFWellLocationID=(select BaseID
						  from @DuplicateWells
						  where DuplicateID=tblShopTickets.KFWellLocationID)

where KFWellLocationID in (select DuplicateID from @DuplicateWells);


-- delete duplicates
select * from @DuplicateWells

delete from tblWellLocation
where KPWellLocationID in (select DuplicateID from @DuplicateWells)