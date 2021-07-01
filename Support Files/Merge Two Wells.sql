-- This script merges two different wells by number even if they are not duplicates.

-- Verify merge info
declare @MergeTargetLeaseName nvarchar(50) = 'Rock',
		@MergeTargetWellNumber nvarchar(50) = '13N',
		@MergeSourceLeaseName nvarchar(50) = 'Rock',
		@MergeSourceWellNumber nvarchar(50) = '13-N',
		@MergeTargetWellId int,
		@MergeSourceWellId int,
		@MatchCount int = 0
		
select @MatchCount = count(*) from Wells inner join Leases on Wells.LeaseID=Leases.LeaseID where WellNumber=@MergeTargetWellNumber and LocationName=@MergeSourceLeaseName
if @MatchCount<1 throw 50000 ,'Could not find source well!',0
if @MatchCount>1 throw 50000 ,'Found more than one source well!',0

select @MatchCount = count(*) from Wells inner join Leases on Wells.LeaseID=Leases.LeaseID where WellNumber=@MergeSourceWellNumber and LocationName=@MergeTargetLeaseName
if @MatchCount<1 throw 50000 ,'Could not find target well!',0
if @MatchCount>1 throw 50000 ,'Found more than one target well!',0

-- Move tickets from source to target
select @MergeSourceWellId = WellID from Wells inner join Leases on Wells.LeaseID=Leases.LeaseID where WellNumber=@MergeSourceWellNumber and LocationName=@MergeSourceLeaseName
select @MergeTargetWellId = WellID from Wells inner join Leases on Wells.LeaseID=Leases.LeaseID where WellNumber=@MergeTargetWellNumber and LocationName=@MergeTargetLeaseName

update DeliveryTickets
set WellID=@MergeTargetWellId
where WellID=@MergeSourceWellId

update pumps
set InstalledInWellID =@MergeTargetWellID
where InstalledInWellID =@MergeSourceWellId

-- Delete source
delete from Wells
where WellID=@MergeSourceWellId


