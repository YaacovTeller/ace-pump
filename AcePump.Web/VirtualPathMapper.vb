Imports AcePump.Common

Public Class VirtualPathMapper
    Implements IVirtualPathMapper

    Private Shared Ctor As Lazy(Of VirtualPathMapper) = New Lazy(Of VirtualPathMapper)(Function() New VirtualPathMapper())
    Public Shared ReadOnly Property Instance As VirtualPathMapper
        Get
            Return Ctor.Value
        End Get
    End Property

    Private Sub New()
    End Sub

    Public Function MapPath(appRelativePath As String) As String Implements IVirtualPathMapper.MapPath
        Return Hosting.HostingEnvironment.MapPath(appRelativePath)
    End Function
End Class
