Namespace Models
    Public Class Lease
        Public Property LeaseID As Integer
        Public Property LocationName As String
        Public Property IgnoreInReporting As Boolean?

        Public Overridable Property Wells As ICollection(Of Well)
    End Class
End Namespace