--Pre DB Audit:
--1 Run all migration scripts!! Make sure on correct DB!
--2 Then run this script on correct DB
--3 Then open the DB Audit script set it to SQLCMD mode (query->sqlcmd mode)
--4 Make sure the name of the db to use in the setvars is the correct one! Change if need be.

--5 Run the post audit migration script, if there is any.
BEGIN TRANSACTION
IF dbo.fnColumnExists('tblShopTickets', 'SalesTaxRate') = 0
BEGIN
   ALTER TABLE tblShopTickets ADD SalesTaxRate DECIMAL (6, 5) NULL; 
   EXEC('UPDATE tblShopTickets SET SalesTaxRate = 0');
   EXEC('ALTER TABLE tblShopTickets ALTER COLUMN SalesTaxRate DECIMAL (6, 5) NOT NULL');
END
GO

Update a set a.SalesTaxRate = CONVERT(DECIMAL(6,5),b.SalesTaxRate )
from tblshoptickets a
inner join tblInvoices b on a.KPShopTicketID = b.KFShopTicketID

drop table tblInvoices

update tblPartRuntimes set Start = null, Finish = null
update tblPartRuntimeSegments set Start = null, Finish = null
update tblPumpRuntimes set Start = null, Finish = null

exec sp_rename 'tblPartRuntimes.PartID', 'TemplatePartDefID';

update tblshoptickets set orderdate = '2014-06-16' where [OrderDate]  =  '0214-06-16';
update tblshoptickets set [ShipDate] = '2014-06-12'  where [ShipDate]  =  '0214-06-12';
update tblshoptickets set [ShipDate] = '2016-12-21' where [ShipDate]  =  '0206-12-21';
update tblshoptickets set PumpFailedDate = '2014-4-10'  where PumpFailedDate =  '0214-04-10';
update tblshoptickets set PumpDispatchedDate = '2014-6-12' where  PumpDispatchedDate  =  '0214-06-12';
update tblshoptickets set shipdate = '2017-09-11' where kpshopticketid = 17893

update tblTemplatePartsJoin set Quantity =0 where KPTemplatePartsJoinID =  22973

update tblLineItems set kfpartID = null where kfpartid not in (select kppartid from tblparts)

update tblParts set KFPartCategory = null where KFPartCategory not in (select KPCategoryID from tblPartsCategory)
update tblParts set KFAssemblyID = null where KFAssemblyID not in (select KPAssemblyID from tblAssemblies)
update tblParts set KFOptionID = null where KFOptionID not in (select ItemTypeID from tblTypes_SoldByOption)
update tblPumps set KFWellLocationID = null where KFWellLocationID not in (select KPWellLocationID from tblWellLocation)
update tblPumps set KFPumpTemplateID = 439 where KFPumpTemplateID not in (select kppumptemplateid from tblPumpTemplates)
update tblRepairTickets set KFParentAssemblyID = null where KFParentAssemblyID not in (select KPRepairID from tblRepairTickets)
update tblRepairTickets set KFPartReplacedID = null where KFPartReplacedID = 0
update tblRepairTickets set KFPartReplacedID = null where KFPartReplacedID not in (select kppartid from tblparts) 
delete from tblUserRoles where UserID not in (select UserID from tblUsers)
delete from tblPartRuntimeSegments
delete from tblPumpRuntimes
delete from tblPartRuntimes
delete from tblTemplatePartsJoin where KFPumpTemplateID not in (select KPPumpTemplateID from tblpumptemplates) 
DROP USER lance
DROP USER bsmith
alter table [tblTypes_InvBallsAndSeatsCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvBallsCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvBarrelCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvCagesCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvHoldDownCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvPlungerCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvRodGuideCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvSeatsCondition] add primary key clustered ([ItemTypeID] asc)
alter table [tblTypes_InvValveRodCondition] add primary key clustered ([ItemTypeID] asc)

exec sp_rename 'tblUsers.PK_tblUsers', 'PK_dbo.tblUsers';  

GO
COMMIT TRANSACTION