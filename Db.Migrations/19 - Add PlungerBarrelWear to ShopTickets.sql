-- CW Case 2121, Add PlungerBarrelWear to Delivery Tickets. Needs to be exactly 306 characters. Allow Nulls.

IF dbo.fnColumnExists('tblShopTickets', 'PlungerBarrelWear') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD PlungerBarrelWear nchar(306) NULL;
END
GO
