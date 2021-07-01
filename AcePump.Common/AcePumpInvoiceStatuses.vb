Imports System.ComponentModel

Public Enum AcePumpInvoiceStatuses
    <Description("None specified")> _
    <InvoiceStatusAccess(UserAccess:=True)> _
    None = 0
    <Description("Ready for QuickBooks")> _
    <InvoiceStatusAccess(UserAccess:=True)> _
    ReadyForQuickbooks = 2
    <Description("In QuickBooks")> _
    <InvoiceStatusAccess(UserAccess:=False)> _
    InQuickbooks = 4
End Enum


