-- BUILD RUNTIMES SCRIPT
--
-- This script deletes all current runtime information in the database and rebuilds it
-- from inspection and delivery ticket data.  The script also verifies the data it generates.
--
-- NOTE: You can set whether the script should save data or verify data by changing the @VerifyData
-- and @SaveData parameters.  These parameters are defined after the workspace creation block.

set nocount on;

-- clear all existing data
truncate table tblPumpRuntimes;
truncate table tblPartRuntimeSegments;
alter table tblPartRuntimeSegments drop constraint FK_tblPartRuntimeSegments_tblPartRuntimes;
truncate table tblPartRuntimes;
alter table tblPartRuntimeSegments add constraint FK_tblPartRuntimeSegments_tblPartRuntimes foreign key (RuntimeID) references tblPartRuntimes (PartRuntimeID);

-- clear previous failed run
if object_id('dbo.__PumpRuntimesWorkspace__', 'U') is not null
  drop table dbo.__PumpRuntimesWorkspace__

if object_id('dbo.__PartRuntimesWorkspace__', 'U') is not null
  drop table dbo.__PartRuntimesWorkspace__

if object_id('dbo.__PartRuntimeSegmentsWorkspace__', 'U') is not null
  drop table dbo.__PartRuntimeSegmentsWorkspace__

if object_id('dbo.__DispatchIdsAfterFailedInspectionWorkspace__', 'U') is not null
  drop table dbo.__DispatchIdsAfterFailedInspectionWorkspace__  

if exists(select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='tblPartRuntimes' and COLUMN_NAME='CandidateRuntimeID') alter table tblPartRuntimes drop column CandidateRuntimeID;

-- workspace
create table __PumpRuntimesWorkspace__ (
	CandidateRuntimeID int identity(1,1) primary key,
	PumpID int,
	RuntimeStartedByTicketID int,
	RuntimeEndedByTicketID int,
	Start date,
	Finish date,
	LengthInDays int
)

create table __PartRuntimesWorkspace__ (
	CandidateRuntimeID int identity(1,1) primary key,
	PumpID int,
	TemplatePartDefID int,
	RuntimeStartedByTicketID int,
	RuntimeEndedByInspectionID int,
	Start date,
	Finish date,
	TotalDaysInGround int
)

create table __PartRuntimeSegmentsWorkspace__ (
	CandidateSegmentID int identity(1,1) primary key,
	RuntimeID int,
	PumpID int,
	TemplatePartDefID int,
	SegmentStartedByTicketID int,
	SegmentEndedByTicketID int,
	Start date,
	Finish date,
	LengthInDays int
)

create table __DispatchIdsAfterFailedInspectionWorkspace__ (
	Id int identity(1,1) primary key,
	DispatchId int, 
	PumpID int, 
	TemplatePartDefID int
)

alter table tblPartRuntimes add CandidateRuntimeID int not null
go

-- Decide how you want the script to run
declare @VerifyData bit = 0,
		@SaveData bit = 0,
		@MatchingRowCount int;

-- PUMPS
---------------------------------------------------------------------------------------
-- Phase 1A: Build Candidates from start/end events
insert into __PumpRuntimesWorkspace__ (PumpID, RuntimeStartedByTicketID, RuntimeEndedByTicketID, Start, Finish, LengthInDays)
select
	case when dtFails.PumpFailedID is not null then dtFails.PumpFailedID else dtDispatches.PumpDispatchedID end,
	dtDispatches.KPShopTicketID,
	dtFails.KPShopTicketID,
	dtDispatches.DispatchedDate,
	dtFails.FailedDate,
	datediff(day,
			 dtDispatches.DispatchedDate,
			 case when dtFails.FailedDate is null then convert(date, GETDATE()) else dtFails.FailedDate end)

from
	(
		select KPShopTicketID, PumpDispatchedID, (case when PumpDispatchedDate is null then TicketDate else PumpDispatchedDate end) as DispatchedDate from tblShopTickets where PumpDispatchedID is not null and PumpDispatchedID <> 0
	) dtDispatches
	inner join (
		select KPShopTicketID, PumpFailedID, (case when PumpFailedDate is null then TicketDate else PumpFailedDate end) as FailedDate from tblShopTickets where PumpFailedID is not null and PumpFailedID <> 0
	) dtFails on dtFails.PumpFailedID = dtDispatches.PumpDispatchedID

-- Phase 1B: Add no start/no end candidates
insert into __PumpRuntimesWorkspace__ (PumpID, RuntimeStartedByTicketID, RuntimeEndedByTicketID, Start, Finish, LengthInDays)
select
	PumpDispatchedID,
	KPShopTicketID,
	null as RuntimeEndedByTicketID,
	(case when PumpDispatchedDate is null then TicketDate else PumpDispatchedDate end) as DispatchedDate,
	null as Finish,
	datediff(day,
		     case when PumpDispatchedDate is null then TicketDate else PumpDispatchedDate end,
			 getdate()) as LengthInDays

from tblShopTickets
where PumpDispatchedID is not null and PumpDispatchedID <> 0

insert into __PumpRuntimesWorkspace__ (PumpID, RuntimeStartedByTicketID, RuntimeEndedByTicketID, Start, Finish, LengthInDays)
select
	PumpFailedID,
	null as RuntimeStartedByTicketID,
	KPShopTicketID,
	null as Start,
	(case when PumpFailedDate is null then TicketDate else PumpFailedDate end) as FailedDate,
	null as LengthInDays

from tblShopTickets
where PumpFailedID is not null and PumpFailedID <> 0

-- Phase 2A: Delete if failed before dispatched
delete from __PumpRuntimesWorkspace__ where LengthInDays <= 0 and LengthInDays is not null

-- Phase 2B: Delete if end is after next start
delete from __PumpRuntimesWorkspace__ where CandidateRuntimeID in (
	select CandidateRuntimeID from __PumpRuntimesWorkspace__ intermediary where Finish > (
		select min(Start)
		from __PumpRuntimesWorkspace__ nextStart
		where nextStart.RuntimeStartedByTicketID=__PumpRuntimesWorkspace__.RuntimeStartedByTicketID and nextStart.PumpID=__PumpRuntimesWorkspace__.PumpID
		  and nextStart.Start>__PumpRuntimesWorkspace__.Start
	)
)

-- Phase 2C: Delete if start is before previous end
delete from __PumpRuntimesWorkspace__ where CandidateRuntimeID in (
	select CandidateRuntimeID from __PumpRuntimesWorkspace__ intermediary where Start < (
		select max(Finish)
		from __PumpRuntimesWorkspace__ prevFinish
		where prevFinish.RuntimeEndedByTicketID=__PumpRuntimesWorkspace__.RuntimeEndedByTicketID and prevFinish.PumpID=__PumpRuntimesWorkspace__.PumpID
		  and prevFinish.Finish<__PumpRuntimesWorkspace__.Finish
	)
)

-- Phase 3A: Delete if not shortest remaining runtime from start
delete from __PumpRuntimesWorkspace__ where CandidateRuntimeID not in (
	select pr.CandidateRuntimeID
	from
		__PumpRuntimesWorkspace__ pr
		inner join
			(select PumpID, RuntimeStartedByTicketID, min(LengthInDays) as Runtime
			 from __PumpRuntimesWorkspace__
			 group by PumpID, RuntimeStartedByTicketID) shortestRuntimes on shortestRuntimes.PumpID = pr.PumpID and shortestRuntimes.RuntimeStartedByTicketID = pr.RuntimeStartedByTicketID and shortestRuntimes.Runtime = LengthInDays
) and Finish is not null and LengthInDays is not null

-- Phase 3B: Delete if not shortest remaining runtime from finish
delete from __PumpRuntimesWorkspace__ where CandidateRuntimeID not in (
	select pr.CandidateRuntimeID
	from
		__PumpRuntimesWorkspace__ pr
		inner join
			(select PumpID, RuntimeEndedByTicketID, min(LengthInDays) as Runtime
			 from __PumpRuntimesWorkspace__
			 group by PumpID, RuntimeEndedByTicketID) shortestRuntimes on shortestRuntimes.PumpID = pr.PumpID and shortestRuntimes.RuntimeEndedByTicketID = pr.RuntimeEndedByTicketID and shortestRuntimes.Runtime = LengthInDays
) and Finish is not null and LengthInDays is not null

-- Phase 4A: Delete null start LengthInDays runtimes if a non-null length runtime exists for same IDs
delete from __PumpRuntimesWorkspace__ where RuntimeEndedByTicketID in (
	select RuntimeEndedByTicketID
	from __PumpRuntimesWorkspace__
	where LengthInDays is not null
) and LengthInDays is null

-- Phase 4B: Arbitrarily null the finish date and length for all remaining same IDs from end
update __PumpRuntimesWorkspace__ set Finish=null, RuntimeEndedByTicketID=null, LengthInDays=null
where CandidateRuntimeID not in (
	select min(CandidateRuntimeID)
	from __PumpRuntimesWorkspace__
	group by RuntimeEndedByTicketID
)

-- Phase 4C: Arbitrarily null the start date and length for all remaining same IDs from start
update __PumpRuntimesWorkspace__ set Start=null, RuntimeStartedByTicketID=null, LengthInDays=null
where CandidateRuntimeID not in (
	select min(CandidateRuntimeID)
	from __PumpRuntimesWorkspace__
	group by RuntimeStartedByTicketID
)

-- Phase 4D: Delete all arbitrarily nulled from both sides
delete from __PumpRuntimesWorkspace__ where RuntimeStartedByTicketID is null and RuntimeEndedByTicketID is null

-- VERIFY RESULTS ------------------------------------------------
if @VerifyData = 0 goto SAVE_PUMP_RUNTIMES
select @MatchingRowCount=count(*)
from
	tblShopTickets dtDispatches
	left join __PumpRuntimesWorkspace__ r on r.RuntimeStartedByTicketID = dtDispatches.KPShopTicketID
where r.CandidateRuntimeID is null and dtDispatches.PumpDispatchedID is not null
if @MatchingRowCount>0 throw 50000,'Found dispatches in the delivery tickets table with no matching runtime start!',0

select @MatchingRowCount=count(*)
from
	tblShopTickets dtFails
	left join __PumpRuntimesWorkspace__ r on r.RuntimeEndedByTicketID = dtFails.KPShopTicketID
where r.CandidateRuntimeID is null and dtFails.PumpFailedID is not null
if @MatchingRowCount>0 throw 50000 ,'Found failures in the delivery tickets table with no matching runtime finish!',0

select @MatchingRowCount=count(*)
from __PumpRuntimesWorkspace__
where RuntimeStartedByTicketID is not null
group by PumpID, RuntimeStartedByTicketID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple runtimes for the same pump with the same started by delivery ticket!',0

select @MatchingRowCount=count(*)
from __PumpRuntimesWorkspace__
where RuntimeEndedByTicketID is not null
group by PumpID, RuntimeEndedByTicketID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple runtimes for the same pump with the same ended by delivery ticket!',0

select @MatchingRowCount=count(*)
from __PumpRuntimesWorkspace__
where RuntimeEndedByTicketID is null and RuntimeStartedByTicketID is null
if @MatchingRowCount>0 throw 50000 ,'Found runtimes with no start or end delivery ticket!',0

-- Save Results
SAVE_PUMP_RUNTIMES:
if @SaveData = 0 goto PART_RUNTIMES
insert into tblPumpRuntimes (PumpID, RuntimeStartedByTicketID, RuntimeEndedByTicketID, Finish, Start, LengthInDays)
select PumpID, RuntimeStartedByTicketID, RuntimeEndedByTicketID, Finish, Start, LengthInDays from __PumpRuntimesWorkspace__

-- Print Summary
print 'Succesfully created ' + convert(varchar(10), @@ROWCOUNT) + ' pump runtimes.';

-- PART RUNTIMES
------------------------------------------------------------------------------------------------------------
PART_RUNTIMES:
-- Phase 1: Build all segments from existing pump runtimes
insert into __PartRuntimeSegmentsWorkspace__ (RuntimeID, TemplatePartDefID, PumpID, SegmentStartedByTicketID, SegmentEndedByTicketID, Start, Finish, LengthInDays)
select
	null as PartRuntimeID,
	tp.KPTemplatePartsJoinID,
	pr.PumpID,
	pr.RuntimeStartedByTicketID,
	pr.RuntimeEndedByTicketID,
	pr.Start,
	pr.Finish,
	pr.LengthInDays

from
	tblPumpRuntimes pr
	inner join tblPumps p on p.KPPumpID=pr.PumpID
	inner join tblTemplatePartsJoin tp on tp.KFPumpTemplateID=p.KFPumpTemplateID
	
-- Phase 2: Create part runtimes from failed inspections
insert into __PartRuntimesWorkspace__ (PumpID, TemplatePartDefID, RuntimeStartedByTicketID, RuntimeEndedByInspectionID, Start, Finish, TotalDaysInGround)
select
	PumpFailedID,
	FailedTemplatePartDefID,
	null as RuntimeStartedByTicketID,
	KPRepairID,
	null as Start,
	(case when PumpFailedDate is null then TicketDate else PumpFailedDate end) as FailedDate,
	null as LengthInDays

from tblRepairTickets i inner join tblShopTickets dt on dt.KPShopTicketID=i.KFShopTicketID
where [Status]='Replace' or ([Status]='Convert' and ReasonRepaired<>'OK') and KFPartFailedID is not null and KFPartFailedID <> 0

-- Phase 3A: Add segments to matching runtimes (arbitrarily select in case of part runtimes which end on the same day)
update __PartRuntimeSegmentsWorkspace__ set RuntimeID=(
	select min(currentRuntime.CandidateRuntimeID) from __PartRuntimesWorkspace__ currentRuntime
	where
		__PartRuntimeSegmentsWorkspace__.PumpID = currentRuntime.PumpID
		and __PartRuntimeSegmentsWorkspace__.TemplatePartDefID = currentRuntime.TemplatePartDefID
		and __PartRuntimeSegmentsWorkspace__.Finish <= currentRuntime.Finish
		and __PartRuntimeSegmentsWorkspace__.Finish > (
			select max(previousRuntime.Finish) 
			from __PartRuntimesWorkspace__ previousRuntime 
			where 
				previousRuntime.Finish < currentRuntime.Finish
				and previousRuntime.TemplatePartDefID = currentRuntime.TemplatePartDefID
				and previousRuntime.PumpID = currentRuntime.PumpID
		)
)

-- Phase 3B: Create new runtimes for any segments which did not match an existing runtime
insert into __PartRuntimesWorkspace__ (PumpID, TemplatePartDefID, RuntimeStartedByTicketID, RuntimeEndedByInspectionID, Start, Finish, TotalDaysInGround)
select distinct
	segment.PumpID,
	segment.TemplatePartDefID,
	segment.SegmentStartedByTicketID,
	null as EndedByInspectionID,
	segment.MyStart,
	null as Finish,
	datediff(day,
		     segment.MyStart,
			 getdate()) as LengthInDays

from (
	select
		segment.PumpID,
		segment.TemplatePartDefID,
		segment.SegmentStartedByTicketID,
		min(segment.Start) over (partition by segment.PumpID, segment.TemplatePartDefID) as FirstStart,
		segment.Start as MyStart

	from __PartRuntimeSegmentsWorkspace__ segment
	where RuntimeID is null
) segment where segment.MyStart=segment.FirstStart

update __PartRuntimeSegmentsWorkspace__ set RuntimeID=(
	select CandidateRuntimeID 
	from __PartRuntimesWorkspace__ runtime 
	where
		runtime.Finish is null
		and runtime.TemplatePartDefID=__PartRuntimeSegmentsWorkspace__.TemplatePartDefID
		and runtime.PumpID=__PartRuntimeSegmentsWorkspace__.PumpID
		and runtime.RuntimeStartedByTicketID=__PartRuntimeSegmentsWorkspace__.SegmentStartedByTicketID
)
where RuntimeID is null

-- Phase 3C: Create new runtimes for any segments which did not match an existing runtime and have no start date
insert into __PartRuntimesWorkspace__ (PumpID, TemplatePartDefID, RuntimeStartedByTicketID, RuntimeEndedByInspectionID, Start, Finish, TotalDaysInGround)
select distinct
	segment.PumpID,
	segment.TemplatePartDefID,
	null as StartedByTicketID,
	null as EndedByInspectionID,
	null as Start,
	null as Finish,
	null as LengthInDays
from __PartRuntimeSegmentsWorkspace__ segment
where RuntimeID is null

update __PartRuntimeSegmentsWorkspace__ set RuntimeID=(
	select CandidateRuntimeID 
	from __PartRuntimesWorkspace__ runtime 
	where
		runtime.Finish is null and runtime.Start is null
		and runtime.TemplatePartDefID=__PartRuntimeSegmentsWorkspace__.TemplatePartDefID
		and runtime.PumpID=__PartRuntimeSegmentsWorkspace__.PumpID
)
where RuntimeID is null

-- Phase 4A: Set runtime start dates from segments
update __PartRuntimesWorkspace__ set Start=(
	select min(segment.Start) 
	from __PartRuntimeSegmentsWorkspace__ segment 
	where 
		segment.RuntimeID=__PartRuntimesWorkspace__.CandidateRuntimeID
)
where Start is null

-- Phase 4B: Calculate total days in ground
update __PartRuntimesWorkspace__
	set TotalDaysInGround=(select sum(LengthInDays) from __PartRuntimeSegmentsWorkspace__ prs where prs.RuntimeID=__PartRuntimesWorkspace__.CandidateRuntimeID and LengthInDays is not null)
	where Start is not null and Finish is not null

-- VERIFY SEGMENT RESULTS ------------------------------------------------
if @VerifyData = 0 goto SAVE_PART_RUNTIMES
select @MatchingRowCount=count(*)
from
	tblShopTickets dtDispatches
	inner join tblPumps p on dtDispatches.PumpDispatchedID = p.KPPumpID
	inner join tblTemplatePartsJoin tp on tp.KFPumpTemplateID = p.KFPumpTemplateID
	left join __PartRuntimeSegmentsWorkspace__ r on r.SegmentStartedByTicketID = dtDispatches.KPShopTicketID and r.TemplatePartDefID=tp.KPTemplatePartsJoinID and r.PumpID=dtDispatches.PumpDispatchedID
where r.CandidateSegmentID is null and dtDispatches.PumpDispatchedID is not null
if @MatchingRowCount>0 throw 50000 ,'Found dispatches in the delivery tickets table with no matching part runtime segment start!',0

select @MatchingRowCount=count(*)
from
	tblShopTickets dtFails
	inner join tblPumps p on dtFails.PumpFailedID = p.KPPumpID
	inner join tblTemplatePartsJoin tp on tp.KFPumpTemplateID = p.KFPumpTemplateID
	left join __PartRuntimeSegmentsWorkspace__ r on r.SegmentEndedByTicketID = dtFails.KPShopTicketID and r.TemplatePartDefID=tp.KPTemplatePartsJoinID and r.PumpID=dtFails.PumpFailedID
where r.CandidateSegmentID is null and dtFails.PumpFailedID is not null
if @MatchingRowCount>0 throw 50000 ,'Found failures in the delivery tickets table with no matching part runtime segment finish!',0

select @MatchingRowCount=count(*)
from __PartRuntimeSegmentsWorkspace__
where SegmentStartedByTicketID is not null
group by TemplatePartDefID, PumpID, SegmentStartedByTicketID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple segments for the same runtime with the same started by delivery ticket!',0

select @MatchingRowCount=count(*)
from __PartRuntimeSegmentsWorkspace__
where SegmentEndedByTicketID is not null
group by RuntimeID, SegmentEndedByTicketID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple segments for the same runtime with the same ended by delivery ticket!',0

select @MatchingRowCount=count(*)
from __PartRuntimeSegmentsWorkspace__
where SegmentEndedByTicketID is null and SegmentStartedByTicketID is null
if @MatchingRowCount>0 throw 50000 ,'Found runtimes with no start or end delivery ticket!',0

-- VERIFY RUNTIME RESULTS ------------------------------------------------
insert into __DispatchIdsAfterFailedInspectionWorkspace__ (DispatchId, PumpID, TemplatePartDefID)
select distinct CandidatePumpDispatchTicketID, PumpID, TemplatePartDefID
from (
	select
		dispatchTicket.KPShopTicketID as CandidatePumpDispatchTicketID,
		inspectionTicket.PumpFailedID as PumpID,
		inspections.FailedTemplatePartDefID as TemplatePartDefID,
		(case when dispatchTicket.PumpDispatchedDate is not null then dispatchTicket.PumpDispatchedDate else dispatchTicket.TicketDate end) as CandidatePumpDispatchDate,
		min(case when dispatchTicket.PumpDispatchedDate is not null then dispatchTicket.PumpDispatchedDate else dispatchTicket.TicketDate end) over(partition by dispatchTicket.PumpDispatchedID, inspections.FailedTemplatePartDefID) as FirstPumpDispatchDate

	from 
		tblRepairTickets inspections
		inner join tblShopTickets inspectionTicket on inspectionTicket.KPShopTicketID = inspections.KFShopTicketID
		inner join tblShopTickets dispatchTicket on dispatchTicket.PumpDispatchedID = inspectionTicket.PumpFailedID

	where (inspections.[Status]='Replace' or (inspections.[Status]='Convert' and inspections.ReasonRepaired<>'OK')) and inspections.KFPartFailedID is not null and inspections.KFPartFailedID <> 0
		  and dispatchTicket.PumpDispatchedID is not null
		  and (case when dispatchTicket.PumpDispatchedDate is not null then dispatchTicket.PumpDispatchedDate else dispatchTicket.TicketDate end) > (case when inspectionTicket.PumpFailedDate is not null then inspectionTicket.PumpFailedDate else inspectionTicket.TicketDate end)
)  x where x.CandidatePumpDispatchDate = x.FirstPumpDispatchDate

select @MatchingRowCount=count(*)
from
	__DispatchIdsAfterFailedInspectionWorkspace__ dtDispatches
	left join (
		select p.KPPumpID as PumpID, tp.KPTemplatePartsJoinID as TemplatePartDefID
		from
			tblPumps p
			inner join tblTemplatePartsJoin tp on tp.KFPumpTemplateID = p.KFPumpTemplateID
	) pumpParts on pumpParts.PumpID = dtDispatches.PumpID and pumpParts.TemplatePartDefID = dtDispatches.TemplatePartDefID
where pumpParts.TemplatePartDefID is null
if @MatchingRowCount>0 throw 50000 ,'Illogical first dispatch after failed inspection generated: Pump does not contain part!',0

select @MatchingRowCount=count(*)
from
	__DispatchIdsAfterFailedInspectionWorkspace__ dtDispatches
	left join __PumpRuntimesWorkspace__ r on r.RuntimeStartedByTicketID = dtDispatches.DispatchId and r.PumpID=dtDispatches.PumpID
where r.CandidateRuntimeID is null
if @MatchingRowCount>0 throw 50000 ,'Illogical first dispatch after failed inspection generated: No pump runtimes exist for this dispatch!',0

select @MatchingRowCount=count(*)
from
	__DispatchIdsAfterFailedInspectionWorkspace__ dtDispatches
	left join __PartRuntimeSegmentsWorkspace__ r on r.SegmentStartedByTicketID = dtDispatches.DispatchId and r.TemplatePartDefID=dtDispatches.TemplatePartDefID and r.PumpID=dtDispatches.PumpID
where r.CandidateSegmentID is null
if @MatchingRowCount>0 throw 50000 ,'Illogical first dispatch after failed inspection generated: No part runtime segments exist for this dispatch!',0

select @MatchingRowCount=count(*)
from
	__DispatchIdsAfterFailedInspectionWorkspace__ dtDispatches
	left join __PartRuntimesWorkspace__ r on r.RuntimeStartedByTicketID = dtDispatches.DispatchId and r.TemplatePartDefID=dtDispatches.TemplatePartDefID and r.PumpID=dtDispatches.PumpID
where r.CandidateRuntimeID is null
if @MatchingRowCount>0 throw 50000 ,'Found first dispatch after failed inspection in the delivery tickets table with no matching part runtime start!',0

select @MatchingRowCount=count(*)
from
	__DispatchIdsAfterFailedInspectionWorkspace__ dtDispatches
	right join __PartRuntimesWorkspace__ r on r.RuntimeStartedByTicketID = dtDispatches.DispatchId and r.TemplatePartDefID=dtDispatches.TemplatePartDefID and r.PumpID=dtDispatches.PumpID
where dtDispatches.DispatchId is null
if @MatchingRowCount>0 throw 50000 ,'Found non first dispatch after failed inspection in the delivery tickets table with a part runtime start!',0

select @MatchingRowCount=count(*)
from
	tblRepairTickets inspections
	inner join tblShopTickets dt on dt.KPShopTicketID=inspections.KFShopTicketID
	left join __PartRuntimesWorkspace__ r on r.RuntimeEndedByInspectionID = inspections.KPRepairID and r.PumpID=dt.PumpFailedID and r.TemplatePartDefID=inspections.FailedTemplatePartDefID
where r.CandidateRuntimeID is null and (inspections.[Status]='Replace' or (inspections.[Status]='Convert' and inspections.ReasonRepaired<>'OK')) and inspections.KFPartFailedID is not null and inspections.KFPartFailedID <> 0
if @MatchingRowCount>0 throw 50000 ,'Found failures in the inspections table with no matching part runtime finish!',0

select @MatchingRowCount=count(*)
from __PartRuntimesWorkspace__
where RuntimeStartedByTicketID is not null
group by PumpID, TemplatePartDefID, RuntimeStartedByTicketID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple runtimes for the same part (inside a pump) with the same started by delivery ticket!',0

select @MatchingRowCount=count(*)
from __PartRuntimesWorkspace__
where RuntimeEndedByInspectionID is not null
group by PumpID, TemplatePartDefID, RuntimeEndedByInspectionID
having count(*) > 1
if @MatchingRowCount>0 throw 50000 ,'Found multiple runtimes for the same part (inside a pump) with the same ended by inspection!',0

-- Save Data
SAVE_PART_RUNTIMES:
if @SaveData = 0 goto CLEANUP_WORKSPACES

insert into tblPartRuntimes (TemplatePartDefID, PumpID, RuntimeStartedByTicketID, RuntimeEndedByInspectionID, Finish, Start, CandidateRuntimeID)
select TemplatePartDefID, PumpID, RuntimeStartedByTicketID, RuntimeEndedByInspectionID, Finish, Start, CandidateRuntimeID from __PartRuntimesWorkspace__
print 'Succesfully created ' + convert(varchar(10), @@ROWCOUNT) + ' part runtimes.';

insert into tblPartRuntimeSegments (RuntimeID, SegmentStartedByTicketID, SegmentEndedByTicketID, Finish, Start, LengthInDays)
select 
	pr.PartRuntimeID, 
	SegmentStartedByTicketID, 
	SegmentEndedByTicketID, 
	seg.Finish, 
	seg.Start,
	LengthInDays 

from 
	__PartRuntimeSegmentsWorkspace__ seg
	inner join tblPartRuntimes pr on pr.CandidateRuntimeID = seg.RuntimeID

print 'Succesfully created ' + convert(varchar(10), @@ROWCOUNT) + ' part runtime segments.';

-- cleanup workspace
CLEANUP_WORKSPACES:
drop table __PartRuntimeSegmentsWorkspace__;
drop table __PumpRuntimesWorkspace__;
drop table __PartRuntimesWorkspace__;
drop table __DispatchIdsAfterFailedInspectionWorkspace__;
alter table tblPartRuntimes drop column CandidateRuntimeID;