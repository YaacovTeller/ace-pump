-- CW: We don't need the IsUserTypedRate on DeliveryTickets anymore because of the changes made to tax rates in case 1450 and case 1453

IF dbo.fnColumnExists('tblShopTickets', 'IsUserTypedRate') = 1
BEGIN
       ALTER TABLE tblShopTickets DROP COLUMN IsUserTypedRate
END
GO