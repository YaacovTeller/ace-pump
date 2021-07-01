-- CW Case 2120, Add Order Time and Ship Time. Allow Nulls.

IF dbo.fnColumnExists('tblShopTickets', 'OrderTime') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD OrderTime DateTime NULL;
END
GO

IF dbo.fnColumnExists('tblShopTickets', 'ShipTime') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD ShipTime DateTime NULL;
END
GO