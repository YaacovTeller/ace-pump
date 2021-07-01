-- Export price book
-- Formulas copied from AcePump.Web.Areas.Employees.Models.DisplayDtos.PartModelBase

declare @ShowPrices bit = 1;

if @ShowPrices = 1
	select
		PartNumber as PartNumber,
		Cost as Cost,
		Cost_AsDecimal / (1.0 - Markup_AsDecimal) as DiscountPrice,
		(Cost_AsDecimal / (1.0 - Markup_AsDecimal)) / (1.0 - convert(decimal(4,2), Discount)) as ListPrice,
		Markup as Markup,
		Discount as Discount

	from
		tblParts

	where
		Active is null or Active = 1

else
	select
		PartNumber as PartNumber,
		(Cost_AsDecimal / (1.0 - Markup_AsDecimal)) / (1.0 - convert(decimal(4,2), Discount)) as ListPrice
		
	from
		tblParts
		
	where
		Active is null or Active = 1