-- FIND TEMPLATE IDS FOR INSPECTIONS
--
-- Inspections did not track which line of a pump template they belonged to
-- until April 1, 2015 (Case 778).  To accomodate for the way Ace uses templates,
-- we added the ability the template ID.  This script finds the right ID for all
-- old inspections and sets it.
--
-- NOTE: In cases where the same part appears on a template twice, quantity is used to
-- distinguish.  If both parts have the same quantity, the script arbitrarily chooses the
-- first one.

-- Phase 1: Find matching template/ticket
update tblRepairTickets set FailedTemplatePartDefID = (
	select top 1 tp.KPTemplatePartsJoinID
	from
		tblShopTickets dt
		inner join tblPumps p on dt.PumpFailedID = p.KPPumpID
		inner join tblTemplatePartsJoin tp on p.KFPumpTemplateID = tp.KFPumpTemplateID

	where
		tblRepairTickets.KFShopTicketID = dt.KPShopTicketID
		and tblRepairTickets.KFPartFailedID	= tp.KFPartsID
		and tblRepairTickets.Quantity = tp.Quantity
)

-- Phase 2: Find any template (template changed since inspection was created)
update tblRepairTickets set FailedTemplatePartDefID = (
	select top 1 KPTemplatePartsJoinID
	from tblTemplatePartsJoin
	where
		tblRepairTickets.KFPartFailedID = KFPartsID
)
where FailedTemplatePartDefID is null and KFPartFailedID is not null

-- Phase 3: Use 0 for template ID (no current templates use this part)
update tblRepairTickets set FailedTemplatePartDefID=0 where FailedTemplatePartDefID is null and KFPartFailedID is not null