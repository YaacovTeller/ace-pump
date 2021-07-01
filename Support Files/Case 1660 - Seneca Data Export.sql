declare @startDate datetime = '1/1/2013',
		@endDate datetime = '12/31/2016';

select
	l.LocationName,
	w.WellNumber,
	dt.TicketDate,
	dt.InvBarrel,
	'?',
	dt.InvSVCages,
	dt.InvSVSeats,
	dt.InvSVBalls,
	'?',
	dt.InvValveRod,
	dt.InvPlunger,
	dt.InvPTVCages,
	dt.InvPTVSeats,
	dt.InvPTVBalls,
	dt.InvRodGuide,
	dt.InvTypeBallandSeat,
	case when pt.PumpType is not null then pt.PumpType else '?' end,
	dt.PumpDispatchedID,
	dt.PumpFailedID

from	
	tblShopTickets dt
	inner join tblCustomers c on c.KPCustomerID = dt.KFCustomerID
	inner join tblWellLocation w on w.KPWellLocationID = dt.KFWellLocationID
	inner join tblLeaseLocations l on l.KPLeaseID = w.KFLeaseID
	inner join tblPumps p on p.KPPumpID = dt.PumpFailedID
	inner join tblPumpTemplates pt on pt.KPPumpTemplateID = p.KFPumpTemplateID

where
	c.CustomerName like '%seneca%'
	and dt.PumpFailedID is not null
	and dt.TicketDate > @startDate and dt.TicketDate < @endDate

order by dt.TicketDate desc