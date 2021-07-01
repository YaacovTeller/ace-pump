-- Verify merge info
declare @MergeSourceCustomerName nvarchar(50) = 'FOURSTAR RESOURCES LLC',
		@MergeTargetCustomerName nvarchar(50) = 'FOURSTAR',
		@MergeSourceCustomerId int,
		@MergeTargetCustomerId int,
		@MatchCount int = 0

select @MatchCount = count(*) from Customers where CustomerName=@MergeSourceCustomerName
if @MatchCount<1 throw 50000 ,'Could not find source customer!',0
if @MatchCount>1 throw 50000 ,'Found more than one source customer!',0

select @MatchCount = count(*) from Customers where CustomerName=@MergeTargetCustomerName
if @MatchCount<1 throw 50000 ,'Could not find target customer!',0
if @MatchCount>1 throw 50000 ,'Found more than one target customer!',0

-- Get IDs
select @MergeSourceCustomerId = CustomerID from Customers where CustomerName=@MergeSourceCustomerName
select @MergeTargetCustomerId = CustomerID from Customers where CustomerName=@MergeTargetCustomerName

-- Move data from source to target
update Wells
set CustomerID=@MergeTargetCustomerId
where CustomerID=@MergeSourceCustomerId

update DeliveryTickets
set CustomerID=@MergeTargetCustomerId
where CustomerID=@MergeSourceCustomerId

update UsernameCustomerAccess
set CustomerID=@MergeTargetCustomerId
where CustomerID=@MergeSourceCustomerId

-- Delete source
delete from Customers
where CustomerID=@MergeSourceCustomerId