UPDATE tblLineItems
SET
    UnitPrice = ROUND( UnitPrice, 2 );
    