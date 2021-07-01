<Query Kind="VBProgram">
  <Connection>
    <ID>673be86e-0b95-4a0f-ac1e-3d46c4d77d2d</ID>
    <Persist>true</Persist>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Development\mkosbie\Ace - EF 6 Test\Ace - EF 6 Test\bin\Release\Ace - EF 6 Test.dll</CustomAssemblyPath>
    <CustomTypeName>AceEf6Test.AcePumpContext</CustomTypeName>
    <AppConfigPath>C:\Development\mkosbie\Ace - EF 6 Test\Ace - EF 6 Test\app.config</AppConfigPath>
    <DisplayName>Ace Pump Release - EF 6</DisplayName>
    <IsProduction>true</IsProduction>
  </Connection>
  <Reference Relative="..\..\Ace - EF 6 Test\Ace - EF 6 Test\bin\Release\Ace - EF 6 Test.dll">C:\Development\mkosbie\Ace - EF 6 Test\Ace - EF 6 Test\bin\Release\Ace - EF 6 Test.dll</Reference>
  <Namespace>AceEf6Test.DbModels</Namespace>
</Query>

Sub Main
    Dim baseQuery As New QueryModel() With {
		.EndDate = Date.Today,
		.StartDate = Date.Today.AddMonths(-1),
		.AdditionalParameters = New Dictionary(Of String, Object)()
	}
	Dim query = New AcePumpQueryModel(baseQuery)
	query.AdditionalParameters("CustomerID") = "6675,6678,6680,6681,6892,6874,6875"
	
	Dim cost As IQueryable(Of TicketLineItemCost) = GetTicketLineItemCost(query)
	cost.Dump()
'	Dim result = (From line In cost
'			Group By line.LineItem.DeliveryTicket.TicketDate Into g = Group
'			Order By TicketDate Ascending
'			Select New ChartDataPoint(Of String) With {
'				.Category = TicketDate,
'				.Value = g.Select(Function(x) x.Cost).DefaultIfEmpty(0D).Sum()
'			}) _
'			.ToList() _
'			.Select(Function(x) New ChartDataPoint(Of String) With {
'						.Category = DateTime.Parse(x.Category).ToShortDateString(),
'						.Value = x.Value
'					}) _
'			.AsQueryable()
			
	
End Sub
	
Function GetTicketLineItemCost(query As AcePumpQueryModel) As IQueryable(Of TicketLineItemCost)
	Dim lines = From line In LineItems
				Where line.DeliveryTicket.TicketDate <= query.EndDate And line.DeliveryTicket.TicketDate >= query.StartDate
				Where line.DeliveryTicket.Well IsNot Nothing OrElse line.DeliveryTicket.Well.Lease IsNot Nothing _
				OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue _
				OrElse Not line.DeliveryTicket.Well.Lease.IgnoreInReporting.Value
				Select New With {
					.LineItem = line,
					.SalexTaxRate = If(line.CollectSalesTax.HasValue AndAlso line.CollectSalesTax.Value,
										line.DeliveryTicket.Invoice.SalesTaxRate + 1D,
										1D),
					.Quantity = If(line.Quantity.HasValue, line.Quantity.Value, 0D),
					.PriceEach = If(line.UnitPrice.HasValue, line.UnitPrice.Value, 0D) * (1 - If(line.UnitDiscount.HasValue, line.UnitDiscount.Value, 0D))
				}
	
	If query.WellID.HasValue Then
		lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.WellID.HasValue AndAlso x.LineItem.DeliveryTicket.WellID.Value = query.WellID.Value)
	End If
	
	If query.LeaseID.HasValue Then
		lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.Well.LeaseID = query.LeaseID.Value And Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.HasValue OrElse Not x.LineItem.DeliveryTicket.Well.Lease.IgnoreInReporting.Value)
	End If
	
	If query.CustomerAccessIDs.Any() Then
		lines = lines.Where(Function(x) x.LineItem.DeliveryTicket.CustomerID.HasValue AndAlso query.CustomerAccessIDs.Contains(x.LineItem.DeliveryTicket.CustomerID.Value))
	End If
	
	If Not String.IsNullOrEmpty(query.ReasonRepaired) Then
		lines = lines.Where(Function(x) x.LineItem.PartInspection.ReasonRepaired = query.ReasonRepaired)
	End If
	
	Dim cost As IQueryable(Of TicketLineItemCost) = From line In lines
													Select New TicketLineItemCost() With {
															.LineItem = line.LineItem,
															.Cost = line.PriceEach * line.Quantity * line.SalexTaxRate
													}
	
	Return cost
End Function

Public Class AcePumpQueryModel
   Private Property InternalQuery As New QueryModel

   Public Property StartDate As Date
       Get
           Return InternalQuery.StartDate
       End Get
       Set(value As Date)
           InternalQuery.StartDate = value
       End Set
   End Property

   Public Property EndDate As Date
       Get
           Return InternalQuery.EndDate
       End Get
       Set(value As Date)
           InternalQuery.EndDate = value
       End Set
   End Property

   Private Const CustomerAccessIDsKey As String = "CustomerID"
   Public Property CustomerAccessIDs As List(Of Integer)
       Get
           If Not AdditionalParameters.ContainsKey(CustomerAccessIDsKey) Then
               AdditionalParameters(CustomerAccessIDsKey) = New List(Of Integer)

           ElseIf TypeOf AdditionalParameters(CustomerAccessIDsKey) Is String Then
               Dim ids As New List(Of Integer)

               If Not String.IsNullOrEmpty(AdditionalParameters(CustomerAccessIDsKey)) Then
                   Dim buffer As Integer

                   Dim idList As String() = AdditionalParameters(CustomerAccessIDsKey).ToString().Split(","c)
                   For Each id As String In idList
                       If Integer.TryParse(id, buffer) Then
                           ids.Add(buffer)
                       End If
                   Next
               End If

               AdditionalParameters(CustomerAccessIDsKey) = ids
           End If

           Return AdditionalParameters(CustomerAccessIDsKey)
       End Get
       Set(value As List(Of Integer))
           AdditionalParameters(CustomerAccessIDsKey) = value
       End Set
   End Property

   Private Const LeaseIDKey As String = "LeaseID"
   Public Property LeaseID As Integer?
       Get
           If Not AdditionalParameters.ContainsKey(LeaseIDKey) Then
               Return Nothing

           ElseIf TypeOf AdditionalParameters(LeaseIDKey) Is String Then
               If String.IsNullOrEmpty(AdditionalParameters(LeaseIDKey)) Then
                   AdditionalParameters(LeaseIDKey) = New Nullable(Of Integer)

               Else
                   AdditionalParameters(LeaseIDKey) = Integer.Parse(AdditionalParameters(LeaseIDKey))
               End If
           End If

           Return AdditionalParameters(LeaseIDKey)
       End Get
       Set(value As Integer?)
           AdditionalParameters(LeaseIDKey) = value
       End Set
   End Property

   Private Const WellIDKey As String = "WellID"
   Public Property WellID As Integer?
       Get
           If Not AdditionalParameters.ContainsKey(WellIDKey) Then
               Return Nothing

           ElseIf TypeOf AdditionalParameters(WellIDKey) Is String Then
               If String.IsNullOrEmpty(AdditionalParameters(WellIDKey)) Then
                   AdditionalParameters(WellIDKey) = New Nullable(Of Integer)

               Else
                   AdditionalParameters(WellIDKey) = Integer.Parse(AdditionalParameters(WellIDKey))
               End If
           End If

           Return AdditionalParameters(WellIDKey)
       End Get
       Set(value As Integer?)
           AdditionalParameters(WellIDKey) = value
       End Set
   End Property

   Private Const ReasonRepairedKey As String = "ReasonRepaired"
   Public Property ReasonRepaired As String
       Get
           If AdditionalParameters.ContainsKey(ReasonRepairedKey) Then
               Return AdditionalParameters(ReasonRepairedKey)
           Else
               Return ""
           End If
       End Get
       Set(value As String)
           AdditionalParameters(ReasonRepairedKey) = value
       End Set
   End Property

   Private Const CategoryIDKey As String = "CategoryID"
   Public Property CategoryID As Integer?
       Get
           If AdditionalParameters.ContainsKey(CategoryIDKey) Then
               If TypeOf AdditionalParameters(CategoryIDKey) Is String Then
                   If String.IsNullOrEmpty(AdditionalParameters(CategoryIDKey)) Then
                       AdditionalParameters(CategoryIDKey) = New Nullable(Of Integer)

                   Else
                       AdditionalParameters(CategoryIDKey) = Integer.Parse(AdditionalParameters(CategoryIDKey))

                   End If
               End If

               Return AdditionalParameters(CategoryIDKey)
           Else
               Return Nothing
           End If
       End Get
       Set(value As Integer?)
           AdditionalParameters(CategoryIDKey) = value
       End Set
   End Property

   Private Const PartIDKey As String = "PartID"
   Public Property PartID As Integer?
       Get
           If AdditionalParameters.ContainsKey(CategoryIDKey) Then
               If TypeOf AdditionalParameters(PartIDKey) Is String Then
                   If String.IsNullOrEmpty(AdditionalParameters(PartIDKey)) Then
                       AdditionalParameters(PartIDKey) = New Nullable(Of Integer)

                   Else
                       AdditionalParameters(PartIDKey) = Integer.Parse(AdditionalParameters(PartIDKey))

                   End If
               End If

               Return AdditionalParameters(PartIDKey)
           Else
               Return Nothing
           End If
       End Get
       Set(value As Integer?)
           AdditionalParameters(PartIDKey) = value
       End Set
   End Property

   Public Property AdditionalParameters As Dictionary(Of String, Object)
       Get
           Return InternalQuery.AdditionalParameters
       End Get
       Set(value As Dictionary(Of String, Object))
           InternalQuery.AdditionalParameters = value
       End Set
   End Property

   Public Sub New(fromQuery As QueryModel)
       InternalQuery = fromQuery
   End Sub
End Class

Public Class QueryModel 
	Public Property StartDate As Date
	Public Property EndDate As Date
	Public Property AdditionalParameters As Dictionary(Of String, Object)
End Class

Public Class TicketLineItemCost
   Public Property LineItem As LineItem

   Public Property Cost As Decimal
End Class