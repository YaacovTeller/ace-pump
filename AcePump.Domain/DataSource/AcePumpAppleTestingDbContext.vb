Namespace DataSource
    Public Class AcePumpAppleTestingDbContext
        Inherits AcePumpContext

        Public Sub New(connStringName As String)
            MyBase.New(connStringName)
        End Sub
    End Class
End Namespace