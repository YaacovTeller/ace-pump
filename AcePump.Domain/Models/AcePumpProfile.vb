Imports Yesod.LinqProviders

Namespace Models
    Public Class AcePumpProfile
        Public Property UserID As Integer
        Public Overridable Property User As AccountDataStoreUser

        Public Property CustomerID As Integer?
        Public Overridable Property Customer As Customer

        Public Overridable Property CustomerAccess As ICollection(Of Customer)

        Private _UsesInventory As Boolean?
        <ComponentModel.DataAnnotations.Schema.NotMapped()>
        Public ReadOnly Property UsesInventory As Boolean
            Get
                If Not _UsesInventory.HasValue Then
                    If CustomerAccess IsNot Nothing AndAlso CustomerAccess.Count > 0 Then
                        _UsesInventory = CustomerAccess.Any(Function(x) x.UsesInventory)
                    Else
                        _UsesInventory = Customer.UsesInventory
                    End If
                End If

                Return _UsesInventory.Value
            End Get
        End Property

        Private _CustomerAccessList As Dictionary(Of String, Integer)
        <System.ComponentModel.DataAnnotations.Schema.NotMapped()> _
        Public ReadOnly Property CustomerAccessList As Dictionary(Of String, Integer)
            Get
                If _CustomerAccessList Is Nothing Then
                    If CustomerAccess IsNot Nothing Then
                        _CustomerAccessList = CustomerAccess.ToDictionary(Function(x) x.CustomerName, Function(x) x.CustomerID)
                    Else
                        _CustomerAccessList = New Dictionary(Of String, Integer)
                    End If

                    If _CustomerAccessList.Count = 0 And CustomerID.HasValue Then
                        _CustomerAccessList.Add(Customer.CustomerName, CustomerID.Value)
                    End If
                End If

                Return _CustomerAccessList
            End Get
        End Property
    End Class
End Namespace