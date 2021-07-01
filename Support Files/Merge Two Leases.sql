-- This script merges two different leases by name even if they are not duplicates.

-- Verify merge info
declare @MergeTargetLeaseName nvarchar(50) = 'UNION BRADLEY',
		@MergeSourceLeaseName nvarchar(50) = 'Bradley Lands',
		@MergeTargetLeaseId int,
		@MergeSourceLeaseId int,
		@MatchCount int = 0
		
select @MatchCount = count(*) from Leases where Leases.LocationName=@MergeSourceLeaseName
if @MatchCount<1 throw 50000 ,'Could not find source lease!',0
if @MatchCount>1 throw 50000 ,'Found more than one source lease!',0

select @MatchCount = count(*) from Leases where LocationName=@MergeTargetLeaseName
if @MatchCount<1 throw 50000 ,'Could not find target lease!',0
if @MatchCount>1 throw 50000 ,'Found more than one target lease!',0

-- Move wells from source to target
select @MergeSourceLeaseId = LeaseID from Leases where LocationName=@MergeSourceLeaseName
select @MergeTargetLeaseId = LeaseID from Leases where LocationName=@MergeTargetLeaseName

update Wells
set LeaseID=@MergeTargetLeaseId
where LeaseID=@MergeSourceLeaseId

-- Delete source
delete from Leases
where LeaseID=@MergeSourceLeaseId