Namespace Models
    Public Class WidgetManagerConfigurationModel
        Public Property CustomerSelectionType As SelectionType
        Public Property CustomersIDsThatMayBeAccessed As Dictionary(Of String, Integer)

        Public Property CustomerID As Integer?
        Public Property CustomerName As String

        Public Property LeaseSelectionType As SelectionType
        Public Property LeaseID As Integer?
        Public Property LeaseName As String

        Public Property WellSelectionType As SelectionType
        Public Property WellID As Integer?
        Public Property WellNumber As String

        Public Property ReasonRepaired As String

        Public Property CategoryID As Integer?
        Public Property CategoryName As String

        Public Property PartTemplateID As Integer?
        Public Property PartTemplateNumber As String

        Public Property StartDate As Date?
        Public Property EndDate As Date?
    End Class
End Namespace